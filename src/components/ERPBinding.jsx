import { useState, useEffect } from 'react';

const ERPBinding = ({ userId, erpAccount, onBind }) => {
  const [erpCode, setErpCode] = useState(erpAccount?.erpCode || '');
  const [uid, setUid] = useState(erpAccount?.uid || '');
  const [upwd, setUpwd] = useState('');
  const [loading, setLoading] = useState(false);
  const [status, setStatus] = useState(erpAccount?.erpCode ? 'Bound' : 'Not Bound');

  useEffect(() => {
    if (erpAccount?.erpCode) {
      setErpCode(erpAccount.erpCode);
      setUid(erpAccount.uid);
      setStatus('Bound');
    }
  }, [erpAccount]);

  const handleBind = async (e) => {
    e.preventDefault();
    setLoading(true);
    
    try {
      // 修正 URL 拼接問題，確保不出現重複的 /Line/line
      let apiUrl = import.meta.env.VITE_API_URL || '/api';
      if (apiUrl.endsWith('/Line')) {
        apiUrl = apiUrl.substring(0, apiUrl.length - 5);
      }
      
      const response = await fetch(`${apiUrl}/Line/bind-erp`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          userId: userId,
          erpCode: erpCode,
          uid: uid,
          upwd: upwd
        })
      });

      const result = await response.json();

      if (response.ok && result.success) {
        setLoading(false);
        setStatus('Bound');
        onBind({ erpCode, uid });
        alert('ERP Account Bound Successfully!');
      } else {
        throw new Error(result.message || 'Failed to bind ERP account');
      }
    } catch (err) {
      console.error('Binding error:', err);
      alert('Binding failed: ' + err.message);
      setLoading(false);
    }
  };

  return (
    <div className="erp-binding-card">
      <h3>ERP Account Integration</h3>
      <p className={`status-badge ${status.toLowerCase()}`}>{status}</p>
      
      {status !== 'Bound' ? (
        <form onSubmit={handleBind}>
          <div className="input-group">
            <label>Level 1 ERP Code (系統代碼)</label>
            <input 
              type="text" 
              value={erpCode} 
              onChange={(e) => setErpCode(e.target.value)} 
              placeholder="例如: CORP_001"
              required
            />
          </div>
          <div className="input-group">
            <label>Level 2 Account (您的帳號)</label>
            <input 
              type="text" 
              value={uid} 
              onChange={(e) => setUid(e.target.value)} 
              placeholder="Enter your account"
              required
            />
          </div>
          <div className="input-group">
            <label>ERP Password (密碼)</label>
            <input 
              type="password" 
              value={upwd} 
              onChange={(e) => setUpwd(e.target.value)} 
              placeholder="Enter password"
              required
            />
          </div>
          <button type="submit" disabled={loading}>
            {loading ? 'Verifying...' : 'Bind Account'}
          </button>
        </form>
      ) : (
        <div className="bound-info">
          <p>Linked to ERP: <strong>{erpCode}</strong></p>
          <p>Account: <strong>{uid}</strong></p>
          <button className="secondary-btn" onClick={() => setStatus('Not Bound')}>Unbind Account</button>
        </div>
      )}
    </div>
  );
};

export default ERPBinding;
