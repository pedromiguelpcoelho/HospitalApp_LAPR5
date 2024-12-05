import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import AddStaffPage from '../../../../Pages/Admin/StaffProfile/AddStafePage'; 
import fetchApi from '../../../../Services/fetchApi';

jest.mock('../../../../Services/fetchApi');

test('renders AddStafePage and submits data successfully', async () => {
  // Simulate the API response
  fetchApi.mockResolvedValueOnce({});

  // Mock the alert function
  window.alert = jest.fn();

  render(
    <MemoryRouter>
      <AddStaffPage />
    </MemoryRouter>
  );

  // Check if the input fields are present
  const firstNameInput = screen.getByPlaceholderText('First Name');
  const lastNameInput = screen.getByPlaceholderText('Last Name');
  const emailInput = screen.getByPlaceholderText('Email');
  const phoneNumberInput = screen.getByPlaceholderText('Phone Number');
  const roleSelect = screen.getByLabelText(/role/i);
 

  
  const submitButton = screen.getByRole('button', { name: /create/i });

  // Simulate user input
  fireEvent.change(firstNameInput, { target: { value: 'Jane' } });
  fireEvent.change(lastNameInput, { target: { value: 'Doe' } });
  fireEvent.change(emailInput, { target: { value: 'jane.doe@example.com' } });
  fireEvent.change(phoneNumberInput, { target: { value: '987654321' } });
  fireEvent.change(roleSelect, { target: { value: 'Doctor' } });
   
  const specializationSelect = await screen.findByLabelText(/specialization/i);


  fireEvent.change(specializationSelect, { target: { value: 'Orthopaedist' } });

  // Simulate form submission
  fireEvent.click(submitButton);

  // Check if the API was called with the correct data
  await waitFor(() => {
    expect(fetchApi).toHaveBeenCalledWith('/staff', {
      method: 'POST',
      body: JSON.stringify({
        firstName: 'Jane',
        lastName: 'Doe',
        email: 'jane.doe@example.com',
        phoneNumber: '987654321',
        role: 'Doctor',
        specialization: 'Orthopaedist',
      }),
    });
  });

  // Check if the success message was displayed
  await waitFor(() => {
    expect(screen.getByText('Staff member added successfully!')).toBeInTheDocument();
  });

  // Restore the original fetch
  fetchApi.mockRestore();
});

test('shows error message if API call fails', async () => {
  // Simulate an API error
  fetchApi.mockRejectedValueOnce(new Error('Failed to add staff member'));

  render(
    <MemoryRouter>
      <AddStaffPage />
    </MemoryRouter>
  );

  // Check if the input fields are present
  const firstNameInput = screen.getByPlaceholderText('First Name');
  const lastNameInput = screen.getByPlaceholderText('Last Name');
  const emailInput = screen.getByPlaceholderText('Email');
  const phoneNumberInput = screen.getByPlaceholderText('Phone Number');
  const roleSelect = screen.getByLabelText(/role/i);
  const submitButton = screen.getByRole('button', { name: /create/i });

  // Simulate user input
  fireEvent.change(firstNameInput, { target: { value: 'Jane' } });
  fireEvent.change(lastNameInput, { target: { value: 'Doe' } });
  fireEvent.change(emailInput, { target: { value: 'jane.doe@example.com' } });
  fireEvent.change(phoneNumberInput, { target: { value: '987654321' } });
  fireEvent.change(roleSelect, { target: { value: 'Doctor' } });
  const specializationSelect = await screen.findByLabelText(/specialization/i);

  fireEvent.change(specializationSelect, { target: { value: 'Orthopaedist' } });

  // Simulate form submission
  fireEvent.click(submitButton);

  // Check if the error message is displayed
  await waitFor(() => {
    expect(screen.getByText('Failed to add staff member')).toBeInTheDocument();
  });

  // Restore the original fetch
  fetchApi.mockRestore();
  
});

