import { useEffect, useState } from 'react';
import liff from '@line/liff';
import ERPBinding from './components/ERPBinding';
import ProductOrder from './components/ProductOrder';
import './App.css';

function App() {
  const [profile, setProfile] = useState(null);
  const [error, setError] = useState(null);
  const [initStatus, setInitStatus] = useState('Initializing...');
  const [activeTab, setActiveTab] = useState('dashboard');
  const [erpAccount, setErpAccount] = useState(null);
  const [appData, setAppData] = useState(null);
  const [recordStatus, setRecordStatus] = useState('Pending');
  const [liffLoggedIn, setLiffLoggedIn] = useState(false);

  // Backend API connection
  const recordAccount = async (profile) => {
    try {
      setRecordStatus('Recording...');
      const apiUrl = import.meta.env.VITE_API_URL;
      const response = await fetch(`${apiUrl}/line/record`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          userId: profile.userId,
          displayName: profile.displayName,
          pictureUrl: profile.pictureUrl,
          statusMessage: profile.statusMessage
        })
      });
      if (response.ok) {
        const result = await response.json();
        setRecordStatus('Success');
        if (result.data && result.data.isErpBound) {
          setErpAccount({ erpId: result.data.erpId });
        }
      } else throw new Error('Failed to record');
    } catch (err) {
      console.error('Record error:', err);
      setRecordStatus('Failed');
    }
  };

  const fetchData = async () => {
    try {
      const apiUrl = import.meta.env.VITE_API_URL;
      const response = await fetch(`${apiUrl}/line/dashboard`);
      if (response.ok) {
        const data = await response.json();
        setAppData(data);
      }
    } catch (err) {
      console.error('Fetch error:', err);
    }
  };

  useEffect(() => {
    const initLiff = async () => {
      try {
        const liffId = import.meta.env.VITE_LIFF_ID;
        if (!liffId || liffId === 'your_liff_id_here') {
          throw new Error('Please set a valid VITE_LIFF_ID in your .env file.');
        }

        await liff.init({ liffId });
        setInitStatus('LIFF Initialized');

        const loggedIn = liff.isLoggedIn();
        setLiffLoggedIn(loggedIn);

        if (loggedIn) {
          const userProfile = await liff.getProfile();
          setProfile(userProfile);
          setInitStatus('Ready');
          
          await recordAccount(userProfile);
          await fetchData();
        } else {
          setInitStatus('Please login to continue');
        }
      } catch (err) {
        console.error('LIFF Error:', err);
        setError(err.message);
        setInitStatus('Initialization failed');
      }
    };

    initLiff();
  }, []);

  const handleLogin = () => {
    liff.login();
  };

  const handleLogout = () => {
    liff.logout();
    window.location.reload();
  };

  const handleErpBind = (data) => {
    setErpAccount(data);
  };

  const handleDemo = () => {
    setProfile({
      userId: 'U_DEMO_12345',
      displayName: 'Demo User (Preview)',
      pictureUrl: 'https://img.freepik.com/free-vector/user-blue-gradient_78370-4692.jpg',
      statusMessage: 'This is a simulation of the LIFF environment'
    });
    setRecordStatus('Success');
    setAppData({
      message: 'Demo: Connected to Mock C# Backend',
      updateTime: new Date().toLocaleString(),
      serverStatus: 'Online',
      notifications: 12
    });
    setInitStatus('Ready');
    setLiffLoggedIn(true);
  };

  return (
    <div className="App">
      <header className="app-header">
        <h1>ERP Order Gateway</h1>
        {profile && (
          <nav className="tabs">
            <button className={activeTab === 'dashboard' ? 'active' : ''} onClick={() => setActiveTab('dashboard')}>Profile</button>
            <button className={activeTab === 'binding' ? 'active' : ''} onClick={() => setActiveTab('binding')}>ERP Bind</button>
            <button className={activeTab === 'order' ? 'active' : ''} onClick={() => setActiveTab('order')}>New Order</button>
          </nav>
        )}
      </header>

      <main className="card">
        {error && !profile && (
          <div className="error">
            <p>Error: {error}</p>
            <button className="demo-btn" onClick={handleDemo} style={{ marginTop: '1rem' }}>
              Preview UI Anyway (Demo Mode)
            </button>
          </div>
        )}

        {!profile && !error && (
          <div className="init-container">
            <div className="spinner"></div>
            <p className="status">{initStatus}</p>
            {!liffLoggedIn && initStatus === 'Please login to continue' && (
              <div className="auth-actions">
                <button className="login-btn" onClick={handleLogin}>Login with LINE</button>
                <div className="divider"><span>OR</span></div>
                <button className="demo-btn" onClick={handleDemo}>Preview UI (Demo Mode)</button>
              </div>
            )}
          </div>
        )}

        {profile && (
          <div className="content-area">
            {activeTab === 'dashboard' && (
              <div className="profile-view">
                <img src={profile.pictureUrl} alt="Profile" className="profile-img" />
                <h2>Welcome, {profile.displayName}</h2>
                
                <div className="backend-info">
                  <p>Account Link: <span className={`status-tag ${recordStatus.toLowerCase()}`}>{recordStatus}</span></p>
                  {appData && <p className="backend-msg">{appData.message}</p>}
                </div>

                <div className="erp-status-card">
                  <p>ERP Connection: <span className={erpAccount ? 'text-success' : 'text-warning'}>
                    {erpAccount ? `Connected (${erpAccount.erpId})` : 'Disconnected'}
                  </span></p>
                </div>
                <button onClick={handleLogout} className="logout-btn">Logout</button>
              </div>
            )}

            {activeTab === 'binding' && (
              <ERPBinding 
                userId={profile.userId} 
                erpAccount={erpAccount}
                onBind={handleErpBind} 
              />
            )}

            {activeTab === 'order' && (
              <ProductOrder erpId={erpAccount?.erpId} />
            )}
          </div>
        )}
      </main>
      
      <footer className="footer-info">
        LIFF Status: {profile ? (liff.isInClient?.() ? 'Native' : 'Web Browser') : 'Initializing...'}
      </footer>
    </div>
  );
}

export default App;
