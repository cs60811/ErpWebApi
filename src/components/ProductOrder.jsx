import { useState, useEffect } from 'react';

const ProductOrder = ({ userId }) => {
  const [products, setProducts] = useState([]);
  const [cart, setCart] = useState([]);
  const [submitting, setSubmitting] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  // Get API URL helper
  const getApiUrl = () => {
    let url = import.meta.env.VITE_API_URL || '/api';
    if (url.endsWith('/Line')) {
      url = url.substring(0, url.length - 5);
    }
    return url;
  };

  useEffect(() => {
    const fetchProducts = async () => {
      setLoading(true);
      try {
        const apiUrl = getApiUrl();
        // 呼叫我們實作的 T357 Product_Query API
        const response = await fetch(`${apiUrl}/Product/query?userId=${userId}`);
        const data = await response.json();
        
        if (response.ok) {
          // T357Response<ProductQueryMaster> 結構中的 MasterData
          setProducts(data.masterData || []);
        } else {
          setError(data.message || 'Failed to fetch products');
        }
      } catch (err) {
        setError('Error connecting to backend');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    if (userId && userId !== 'U_DEMO_12345') {
      fetchProducts();
    } else {
      // Demo mode fallback
      setProducts([
        { prodID: 'DEMO-001', prodName: 'Demo Product 1', unit: 'PCS', bCurrStock: 100 },
        { prodID: 'DEMO-002', prodName: 'Demo Product 2', unit: 'BOX', bCurrStock: 50 },
      ]);
    }
  }, [userId]);

  const addToCart = (product) => {
    setCart([...cart, { ...product, cartId: Date.now() }]);
  };

  const removeFromCart = (cartId) => {
    setCart(cart.filter(item => item.cartId !== cartId));
  };

  const handleSubmitOrder = async () => {
    setSubmitting(true);
    // 未來實作 Order_Create 介接
    setTimeout(() => {
      setSubmitting(false);
      setCart([]);
      alert('Order Placed! (This will be sent to T357 Order_Create in the next phase)');
    }, 1500);
  };

  const total = cart.length; // Simplified for T357 demo

  return (
    <div className="order-system">
      <div className="products-grid">
        {loading && <p>Loading products from ERP...</p>}
        {error && <p className="error-msg">{error}</p>}
        {!loading && !error && products.length === 0 && <p>No products found in ERP.</p>}
        
        {products.map(product => (
          <div key={product.prodID} className="product-card">
            <div className="prod-img-placeholder">📦</div>
            <h4>{product.prodName}</h4>
            <p className="prod-id">{product.prodID}</p>
            <p className="stock">Stock: {product.bCurrStock} {product.unit}</p>
            <button onClick={() => addToCart(product)}>Add to Order</button>
          </div>
        ))}
      </div>

      <div className="cart-section">
        <h3>Current Order ({cart.length})</h3>
        {cart.length === 0 ? (
          <p className="empty-msg">No items selected.</p>
        ) : (
          <>
            <ul className="cart-list">
              {cart.map(item => (
                <li key={item.cartId}>
                  <span>{item.prodName}</span>
                  <span className="qty">1 {item.unit}</span>
                  <button className="remove-btn" onClick={() => removeFromCart(item.cartId)}>×</button>
                </li>
              ))}
            </ul>
            <div className="cart-footer">
              <button 
                className="checkout-btn" 
                onClick={handleSubmitOrder}
                disabled={submitting}
              >
                {submitting ? 'Processing...' : 'Submit to ERP'}
              </button>
            </div>
          </>
        )}
      </div>
    </div>
  );
};

export default ProductOrder;
