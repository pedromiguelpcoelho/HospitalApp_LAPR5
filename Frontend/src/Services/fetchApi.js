import Cookies from 'js-cookie';
import config from './config';

const fetchApi = async (endpoint, options = {}, searchParams = {}) => {
    const token = Cookies.get('token');
    const headers = {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`,
        ...options.headers,
    };

    const queryString = new URLSearchParams(searchParams).toString();
    const url = queryString ? `${config.BASE_URL}${endpoint}?${queryString}` : `${config.BASE_URL}${endpoint}`;

    const response = await fetch(url, {
        ...options,
        headers,
    });

    const contentType = response.headers.get('content-type');
    if (!response.ok) {
        if (contentType && contentType.includes('application/json')) {
            const error = await response.json();
            throw new Error(error.message || 'Something went wrong');
        } else {
            const errorText = await response.text();
            throw new Error(`Unexpected response: ${errorText}`);
        }
    }

    if (contentType && contentType.includes('application/json')) {
        return response.json();
    } else {
        return response.text();
    }
};

export default fetchApi;