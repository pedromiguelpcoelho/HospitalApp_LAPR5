import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import AddPatientProfilePage from '../../../../Pages/Admin/PatientProfile/AddPatientProfilePage';
import fetchApi from '../../../../Services/fetchApi';

jest.mock('../../../../Services/fetchApi');

test('renders AddPatientProfilePage and submits data', async () => {
  // Simulate the API response
  fetchApi.mockResolvedValueOnce({});

  // Mock the alert function
  window.alert = jest.fn();

  render(
    <MemoryRouter>
      <AddPatientProfilePage />
    </MemoryRouter>
  );

  // Check if the input fields are present
  const firstNameInput = screen.getByPlaceholderText('First Name');
  const lastNameInput = screen.getByPlaceholderText('Last Name');
  const fullNameInput = screen.getByPlaceholderText('Full Name');
  const dobInput = screen.getByPlaceholderText('Date of Birth');
  const emailInput = screen.getByPlaceholderText('Email');
  const contactInfoInput = screen.getByPlaceholderText('Contact Information');
  const submitButton = screen.getByRole('button', { name: /create/i });

  // Simulate user input
  fireEvent.change(firstNameInput, { target: { value: 'John' } });
  fireEvent.change(lastNameInput, { target: { value: 'Doe' } });
  fireEvent.change(fullNameInput, { target: { value: 'John Doe' } });
  fireEvent.change(dobInput, { target: { value: '1990-01-01' } });
  fireEvent.change(emailInput, { target: { value: 'john.doe@example.com' } });
  fireEvent.change(contactInfoInput, { target: { value: '123456789' } });

  // Simulate form submission
  fireEvent.click(submitButton);

  // Check if the API was called with the correct data
  await waitFor(() => {
    expect(fetchApi).toHaveBeenCalledWith('/PatientProfile', {
      method: 'POST',
      body: JSON.stringify({
        firstName: 'John',
        lastName: 'Doe',
        fullName: 'John Doe',
        dateOfBirth: '1990-01-01',
        email: 'john.doe@example.com',
        contactInformation: '123456789'
      }),
    });
  });

  // Check if the alert was called with the correct message
  await waitFor(() => {
    expect(screen.getByText('Patient profile created successfully!')).toBeInTheDocument();
  });

  // Restore the original fetch
  fetchApi.mockRestore();
});

test('shows loading indicator while creating patient profile', async () => {
  fetchApi.mockResolvedValueOnce({});

  render(
    <MemoryRouter>
      <AddPatientProfilePage />
    </MemoryRouter>
  );

  const firstNameInput = screen.getByPlaceholderText('First Name');
  const lastNameInput = screen.getByPlaceholderText('Last Name');
  const fullNameInput = screen.getByPlaceholderText('Full Name');
  const dobInput = screen.getByPlaceholderText('Date of Birth');
  const emailInput = screen.getByPlaceholderText('Email');
  const contactInfoInput = screen.getByPlaceholderText('Contact Information');
  const submitButton = screen.getByRole('button', { name: /create/i });

  fireEvent.change(firstNameInput, { target: { value: 'John' } });
  fireEvent.change(lastNameInput, { target: { value: 'Doe' } });
  fireEvent.change(fullNameInput, { target: { value: 'John Doe' } });
  fireEvent.change(dobInput, { target: { value: '1990-01-01' } });
  fireEvent.change(emailInput, { target: { value: 'john.doe@example.com' } });
  fireEvent.change(contactInfoInput, { target: { value: '123456789' } });

  fireEvent.click(submitButton);

  expect(screen.getByText('Creating...')).toBeInTheDocument();

  await waitFor(() => {
    expect(fetchApi).toHaveBeenCalledWith('/PatientProfile', {
      method: 'POST',
      body: JSON.stringify({
        firstName: 'John',
        lastName: 'Doe',
        fullName: 'John Doe',
        dateOfBirth: '1990-01-01',
        email: 'john.doe@example.com',
        contactInformation: '123456789'
      }),
    });
  });
});

test('shows error message when API call fails', async () => {
  fetchApi.mockRejectedValueOnce(new Error('Failed to create profile'));

  render(
    <MemoryRouter>
      <AddPatientProfilePage />
    </MemoryRouter>
  );

  const firstNameInput = screen.getByPlaceholderText('First Name');
  const lastNameInput = screen.getByPlaceholderText('Last Name');
  const fullNameInput = screen.getByPlaceholderText('Full Name');
  const dobInput = screen.getByPlaceholderText('Date of Birth');
  const emailInput = screen.getByPlaceholderText('Email');
  const contactInfoInput = screen.getByPlaceholderText('Contact Information');
  const submitButton = screen.getByRole('button', { name: /create/i });

  fireEvent.change(firstNameInput, { target: { value: 'John' } });
  fireEvent.change(lastNameInput, { target: { value: 'Doe' } });
  fireEvent.change(fullNameInput, { target: { value: 'John Doe' } });
  fireEvent.change(dobInput, { target: { value: '1990-01-01' } });
  fireEvent.change(emailInput, { target: { value: 'john.doe@example.com' } });
  fireEvent.change(contactInfoInput, { target: { value: '123456789' } });

  fireEvent.click(submitButton);

  await waitFor(() => {
    expect(fetchApi).toHaveBeenCalledWith('/PatientProfile', {
      method: 'POST',
      body: JSON.stringify({
        firstName: 'John',
        lastName: 'Doe',
        fullName: 'John Doe',
        dateOfBirth: '1990-01-01',
        email: 'john.doe@example.com',
        contactInformation: '123456789'
      }),
    });
  });

  await waitFor(() => {
    expect(screen.queryByText('Creating...')).not.toBeInTheDocument();
    expect(screen.getByText('Failed to create profile')).toBeInTheDocument();
  });
});
