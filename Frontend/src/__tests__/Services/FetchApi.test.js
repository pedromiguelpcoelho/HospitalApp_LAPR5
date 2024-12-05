import fetchApi from '../../Services/fetchApi';
import Cookies from 'js-cookie';
import config from './../../Services/config';

// Mock `Cookies` para simular o token
jest.mock('js-cookie', () => ({
    get: jest.fn(),
}));

// Mock global `fetch`
global.fetch = jest.fn();

describe('fetchApi function', () => {
    beforeEach(() => {
        // Certifique-se de que o mock é resetado antes de cada teste
        Cookies.get.mockReturnValue('fake-token');
        jest.clearAllMocks();
    });

    it('should call fetch with correct headers and URL', async () => {
        const endpoint = '/test-endpoint';
        const options = { method: 'GET' };
        const searchParams = { param1: 'value1', param2: 'value2' };
    
        // Mocking a successful fetch response
        fetch.mockResolvedValueOnce({
            ok: true,
            headers: { get: () => 'application/json' },
            json: async () => ({ success: true }),
        });

        // Chamando a função fetchApi
        const result = await fetchApi(endpoint, options, searchParams);

        // Construir a URL usando o BASE_URL do config corretamente
        const expectedUrl = `${config.BASE_URL}/test-endpoint?param1=value1&param2=value2`;

        expect(fetch).toHaveBeenCalledWith(
            expectedUrl,
            expect.objectContaining({
                method: 'GET',
                headers: expect.objectContaining({
                    'Content-Type': 'application/json',
                    'Authorization': 'Bearer fake-token',
                }),
            })
        );
        expect(result).toEqual({ success: true });
    });

    it('should throw an error for non-OK response with JSON body', async () => {
        fetch.mockResolvedValueOnce({
            ok: false,
            headers: { get: () => 'application/json' },
            json: async () => ({ message: 'Unauthorized' }),
        });

        await expect(fetchApi('/error-endpoint')).rejects.toThrow('Unauthorized');
    });

    it('should throw an error for non-OK response with text body', async () => {
        fetch.mockResolvedValueOnce({
            ok: false,
            headers: { get: () => 'text/plain' },
            text: async () => 'Error occurred',
        });

        await expect(fetchApi('/error-endpoint')).rejects.toThrow('Unexpected response: Error occurred');
    });

    it('should return text when content-type is not JSON', async () => {
        fetch.mockResolvedValueOnce({
            ok: true,
            headers: { get: () => 'text/plain' },
            text: async () => 'Plain text response',
        });

        const result = await fetchApi('/text-endpoint');
        expect(result).toBe('Plain text response');
    });
});
