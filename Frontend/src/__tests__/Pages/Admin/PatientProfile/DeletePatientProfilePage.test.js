import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import DeletePatientProfilePage from '../../../../Pages/Admin/PatientProfile/DeletePatientProfilePage';
import fetchApi from '../../../../Services/fetchApi';

jest.mock('../../../../Services/fetchApi');

test('renders DeletePatientProfilePage and deletes data', async () => {
  // Simulate the API response
  fetchApi.mockResolvedValueOnce({ message: 'Profile deleted successfully!' });

  // Mock the confirm and alert functions
  window.confirm = jest.fn().mockImplementation(() => true);
  window.alert = jest.fn();

  render(
    <MemoryRouter>
      <DeletePatientProfilePage />
    </MemoryRouter>
  );

  // Check if the form is present
  const selectInput = screen.getByRole('combobox');
  const deleteButton = screen.getByRole('button', { name: /delete profile/i });

  // Simulate user input
  fireEvent.change(selectInput, { target: { value: 'fullName' } });
  const fullNameInput = screen.getByPlaceholderText('Full Name');
  fireEvent.change(fullNameInput, { target: { value: 'John Doe' } });

  // Simulate form submission
  fireEvent.click(deleteButton);

  // Check if the confirm dialog was shown
  expect(window.confirm).toHaveBeenCalledWith('Are you sure you want to delete this patient profile?');

  // Check if the API was called with the correct data
  await waitFor(() => {
    expect(fetchApi).toHaveBeenCalledWith('/PatientProfile/delete?fullName=John+Doe', {
      method: 'DELETE',
    });
  });

  // Check if the alert was called with the correct message
  await waitFor(() => {
    expect(window.alert).toHaveBeenCalledWith('Profile deleted successfully!');
  });

  // Restore the original fetch
  fetchApi.mockRestore();
});