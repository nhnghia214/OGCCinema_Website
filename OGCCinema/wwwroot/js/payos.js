// payos.js - Tích hợp PayOS API
const PAYOS_CONFIG = {
    clientId: '30b99b50-d4d6-4682-a6b3-9470d8ce4ade',
    apiKey: '41be4fc9-ca2c-48e2-88d5-be44e2137ee6',
    checksumKey: '285d5eae63fe6194a06e5abeeff018323c3a1ca630976ce29f4be9ba1a7f2368',
    baseUrl: 'https://api.payos.vn/v2/payment-requests'
};

// Tạo payment link
async function createPaymentLink(orderData) {
    const paymentData = {
        orderCode: Date.now(), // Mã đơn hàng unique
        amount: orderData.totalAmount,
        description: `Thanh toán đơn hàng ${orderData.orderCode}`,
        items: orderData.items.map(item => ({
            name: item.name,
            quantity: item.quantity,
            price: item.price
        })),
        returnUrl: `${window.location.origin}/payment-success`,
        cancelUrl: `${window.location.origin}/payment-cancel`
    };

    try {
        const response = await fetch(`${PAYOS_CONFIG.baseUrl}/v2/payment-requests`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'x-client-id': PAYOS_CONFIG.clientId,
                'x-api-key': PAYOS_CONFIG.apiKey
            },
            body: JSON.stringify(paymentData)
        });

        const result = await response.json();

        if (result.code === '00') {
            return {
                checkoutUrl: result.data.checkoutUrl,
                qrCode: result.data.qrCode,
                orderCode: result.data.orderCode
            };
        } else {
            throw new Error(result.desc || 'Tạo payment link thất bại');
        }
    } catch (error) {
        console.error('PayOS Error:', error);
        throw error;
    }
}

// Kiểm tra trạng thái thanh toán
async function checkPaymentStatus(orderCode) {
    try {
        const response = await fetch(`${PAYOS_CONFIG.baseUrl}/v2/payment-requests/${orderCode}`, {
            headers: {
                'x-client-id': PAYOS_CONFIG.clientId,
                'x-api-key': PAYOS_CONFIG.apiKey
            }
        });

        const result = await response.json();
        return result.data;
    } catch (error) {
        console.error('Check payment status error:', error);
        throw error;
    }
}