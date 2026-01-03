import { useState } from 'react';

const ProductOrder = ({ erpId }) => {
  const [cart, setCart] = useState([]);
  const [submitting, setSubmitting] = useState(false);

  const products = [
    { id: 1, name: 'Premium Wireless Headphones', price: 2999, image: 'https://images.unsplash.com/photo-1505740420928-5e560c06d30e?w=200&h=200&fit=crop' },
    { id: 2, name: 'Mechanical Gaming Keyboard', price: 1500, image: 'https://images.unsplash.com/photo-1511467687858-23d96c32e4ae?w=200&h=200&fit=crop' },
    { id: 3, name: '4K Ultra HD Monitor', price: 8500, image: 'https://images.unsplash.com/photo-1527443224154-c4a3942d3acf?w=200&h=200&fit=crop' },
    { id: 4, name: 'Ergonomic Office Chair', price: 4200, image: 'https://images.unsplash.com/photo-1505843490538-5133c6c7d0e1?w=200&h=200&fit=crop' },
  ];

  const addToCart = (product) => {
    setCart([...cart, { ...product, cartId: Date.now() }]);
  };

  const removeFromCart = (cartId) => {
    setCart(cart.filter(item => item.cartId !== cartId));
  };

  const handleSubmitOrder = async () => {
    if (!erpId) {
      alert('Please bind your ERP account first!');
      return;
    }
    
    setSubmitting(true);
    // Simulate API call to ERP WebAPI /order/create
    const orderData = {
      customerId: erpId,
      items: cart.map(item => ({ id: item.id, qty: 1 })),
      timestamp: new Date().toISOString()
    };
    
    console.log('Sending order to ERP:', orderData);
    
    setTimeout(() => {
      setSubmitting(false);
      setCart([]);
      alert('Order Placed Successfully! Sent to ERP WebAPI.');
    }, 2000);
  };

  const total = cart.reduce((sum, item) => sum + item.price, 0);

  return (
    <div className="order-system">
      <div className="products-grid">
        {products.map(product => (
          <div key={product.id} className="product-card">
            <img src={product.image} alt={product.name} />
            <h4>{product.name}</h4>
            <p className="price">${product.price}</p>
            <button onClick={() => addToCart(product)}>Add to Cart</button>
          </div>
        ))}
      </div>

      <div className="cart-section">
        <h3>Shopping Cart ({cart.length})</h3>
        {cart.length === 0 ? (
          <p className="empty-msg">Your cart is empty.</p>
        ) : (
          <>
            <ul className="cart-list">
              {cart.map(item => (
                <li key={item.cartId}>
                  <span>{item.name}</span>
                  <span className="price">${item.price}</span>
                  <button className="remove-btn" onClick={() => removeFromCart(item.cartId)}>×</button>
                </li>
              ))}
            </ul>
            <div className="cart-footer">
              <div className="total">
                <span>Total:</span>
                <span>${total}</span>
              </div>
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
