import React from 'react';
import { render, screen, fireEvent } from '@testing-library/react';
import { BrowserRouter } from 'react-router-dom';  // Importa o BrowserRouter
import SignUpForm from '../Components/Auth/SignUpForm';

test('renders SignUpForm and submits data', () => {
  const handleSignup = jest.fn();

  // Envolva o componente com BrowserRouter para fornecer o contexto de roteamento
  render(
    <BrowserRouter>
      <SignUpForm onSignup={handleSignup} />
    </BrowserRouter>
  );

  // Verifica se os campos de entrada e a checkbox estão presentes
  const emailInput = screen.getByPlaceholderText('Email');
  const passwordInput = screen.getByPlaceholderText('Password');
  const nameInput = screen.getByPlaceholderText('Name');
  const termsCheckbox = screen.getByLabelText(/I agree to the terms and conditions/i);
  const submitButton = screen.getByText('Register');

  // Simula a entrada do usuário
  fireEvent.change(emailInput, { target: { value: 'test@example.com' } });
  fireEvent.change(passwordInput, { target: { value: 'password123' } });
  fireEvent.change(nameInput, { target: { value: 'Test User' } });
  fireEvent.click(termsCheckbox); // Marca a checkbox

  // Simula o envio do formulário
  fireEvent.click(submitButton);

  // Verifica se a função de callback foi chamada com os dados corretos
  expect(handleSignup).toHaveBeenCalledWith('test@example.com', 'password123', 'Test User');
});
