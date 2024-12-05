import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import LoginPage from '../Pages/Auth/LoginPage';
import fetchApi from '../Services/fetchApi';

jest.mock('../Services/fetchApi');

test('renders LoginPage and submits data', async () => {
  const setUser = jest.fn();

  // Simula a resposta da API de autenticação
  fetchApi.mockResolvedValueOnce({
    token: 'fake-token',
    role: 'Admins',
    name: 'Test User'
  });

  render(
      <MemoryRouter>
        <LoginPage setUser={setUser} />
      </MemoryRouter>
  );

  // Verifica se os campos de entrada estão presentes
  const emailInput = screen.getByPlaceholderText('Email');
  const passwordInput = screen.getByPlaceholderText('Password');
  const submitButton = screen.getByRole('button', { name: /login/i });

  // Simula a entrada do usuário
  fireEvent.change(emailInput, { target: { value: 'test@example.com' } });
  fireEvent.change(passwordInput, { target: { value: 'password123' } });

  // Simula o envio do formulário
  fireEvent.click(submitButton);

  // Verifica se a função de callback foi chamada
  await waitFor(() => {
    expect(setUser).toHaveBeenCalledWith({
      email: 'test@example.com',
      role: 'Admins',
      name: 'Test User'
    });
  });

  // Restaura o fetch original
  fetchApi.mockRestore();
});