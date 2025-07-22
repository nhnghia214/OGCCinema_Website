document.addEventListener('DOMContentLoaded', () => {
    const cartItemsContainer = document.getElementById('cart-items');
    const cartTotal = document.getElementById('cart-total');
    const paymentMethod = document.getElementById('payment-method');
    const payButton = document.getElementById('pay-button');
    let cart = JSON.parse(localStorage.getItem('cart')) || [];

    // Hàm render danh sách món trong giỏ hàng
    function renderCart() {
        cartItemsContainer.innerHTML = '';
        if (cart.length === 0) {
            cartItemsContainer.innerHTML = '<p>Giỏ hàng trống.</p>';
            cartTotal.textContent = '0 VNĐ';
            return;
        }
        cart.forEach(item => {
            const itemElement = document.createElement('div');
            itemElement.classList.add('cart-item');
            itemElement.innerHTML = `
                <img src="/image/dichvu/${item.id}.jpg" alt="${item.name}" />
                <div class="item-details">
                    <h5>${item.name}</h5>
                    <p>Giá: ${item.price.toLocaleString()} VNĐ</p>
                    <p>Số lượng: <span class="item-quantity">${item.quantity}</span></p>
                </div>
                <div class="quantity-controls">
                    <button class="btn btn-outline-secondary decrease-quantity" data-id="${item.id}">-</button>
                    <button class="btn btn-outline-secondary increase-quantity" data-id="${item.id}">+</button>
                    <span class="remove-item" data-id="${item.id}">X</span>
                </div>
            `;
            cartItemsContainer.appendChild(itemElement);
        });
        updateCartTotal();
    }

    // Hàm cập nhật tổng tiền
    function updateCartTotal() {
        const total = cart.reduce((sum, item) => sum + item.price * item.quantity, 0);
        cartTotal.textContent = `${total.toLocaleString()} VNĐ`;
        return total;
    }

    // Xử lý tăng số lượng
    cartItemsContainer.addEventListener('click', (e) => {
        if (e.target.classList.contains('increase-quantity')) {
            const id = e.target.getAttribute('data-id');
            const item = cart.find(i => i.id === id);
            if (item) {
                item.quantity += 1;
                localStorage.setItem('cart', JSON.stringify(cart));
                renderCart();
            }
        }
    });

    // Xử lý giảm số lượng
    cartItemsContainer.addEventListener('click', (e) => {
        if (e.target.classList.contains('decrease-quantity')) {
            const id = e.target.getAttribute('data-id');
            const item = cart.find(i => i.id === id);
            if (item && item.quantity > 1) {
                item.quantity -= 1;
                localStorage.setItem('cart', JSON.stringify(cart));
                renderCart();
            }
        }
    });

    // Xử lý xóa món
    cartItemsContainer.addEventListener('click', (e) => {
        if (e.target.classList.contains('remove-item')) {
            const id = e.target.getAttribute('data-id');
            cart = cart.filter(item => item.id !== id);
            localStorage.setItem('cart', JSON.stringify(cart));
            renderCart();
        }
    });

    // Xử lý thanh toán
    payButton.addEventListener('click', async () => {
        const total = updateCartTotal();

        console.log('Cart data:', cart);
        console.log('Total:', total);

        if (!cart.length || total <= 0) {
            alert('Giỏ hàng trống hoặc tổng tiền không hợp lệ!');
            return;
        }

        const validItems = cart.filter(item => item.id && item.name && item.price > 0 && item.quantity > 0);
        if (validItems.length === 0) {
            alert('Không có sản phẩm hợp lệ trong giỏ hàng!');
            return;
        }

        const method = paymentMethod.value;
        const orderData = {
            amount: total,
            description: `Thanh toán ${new Date().toLocaleDateString()}`,
            items: validItems.map(item => ({
                id: item.id || '',
                name: item.name,
                price: parseInt(item.price),
                quantity: parseInt(item.quantity)
            })),
            method: method
        };

        console.log('Order data to send:', orderData);
        console.log('JSON string:', JSON.stringify(orderData));

        payButton.disabled = true;
        payButton.textContent = 'Đang xử lý...';

        try {
            const response = await fetch('/api/MonAn/payment/create', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(orderData)
            });

            console.log('Response status:', response.status);
            const responseText = await response.text();
            console.log('Response text:', responseText);

            if (!response.ok) {
                throw new Error(`Lỗi từ server: ${responseText}`);
            }

            const result = JSON.parse(responseText);
            console.log('Payment result:', result);

            if (result.paymentUrl) {
                const orderCode = Date.now().toString(); // Tạo orderCode tạm thời
                window.location.href = `/MonAn/PaymentQR?paymentUrl=${encodeURIComponent(result.paymentUrl)}&orderCode=${orderCode}`;
                localStorage.setItem('cart', JSON.stringify(cart)); // Lưu tạm cart
            } else {
                throw new Error('PAYOS không trả về mã QR hoặc URL hợp lệ.');
            }
        } catch (error) {
            console.error('Lỗi khi tạo thanh toán:', error);
            alert(`Đã có lỗi xảy ra: ${error.message}`);
        } finally {
            payButton.disabled = false;
            payButton.textContent = 'Thanh toán';
        }
    });

    // Render giỏ hàng khi tải trang
    renderCart();
    console.log("Dữ liệu giỏ hàng:", localStorage.getItem("cart"));
});











