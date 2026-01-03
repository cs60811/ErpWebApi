import { useState, useEffect } from 'react';

const ERPBinding = ({ userId, erpAccount, onBind }) => {
  const [erpId, setErpId] = useState(erpAccount?.erpId || '');
  const [password, setPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [status, setStatus] = useState(erpAccount?.erpId ? 'Bound' : 'Not Bound');

  // Update local state if erpAccount prop changes
  useEffect(() => {
    if (erpAccount?.erpId) {
      setErpId(erpAccount.erpId);
      setStatus('Bound');
    }
  }, [erpAccount]);

  const handleBind = async (e) => {
    e.preventDefault();
    setLoading(true);
    
    try {
      const apiUrl = import.meta.env.VITE_API_URL;
      const response = await fetch(`${apiUrl}/line/bind-erp`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          userId: userId,
          erpId: erpId,
          password: password
        })
      });

      const result = await response.json();

      if (response.ok && result.success) {
        setLoading(false);
        setStatus('Bound');
        onBind({ erpId: result.erpId });
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
            <label>ERP Customer ID</label>
            <input 
              type="text" 
              value={erpId} 
              onChange={(e) => setErpId(e.target.value)} 
              placeholder="Enter your ERP ID"
              required
            />
          </div>
          <div className="input-group">
            <label>ERP Password / PIN</label>
            <input 
              type="password" 
              value={password} 
              onChange={(e) => setPassword(e.target.value)} 
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
          <p>Linked to ERP ID: <strong>{erpId || 'ERP-88291'}</strong></p>
          <button className="secondary-btn" onClick={() => setStatus('Not Bound')}>Unbind Account</button>
        </div>
      )}
    </div>
  );
};

export default ERPBinding;
