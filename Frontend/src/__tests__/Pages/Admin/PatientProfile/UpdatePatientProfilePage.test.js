import React from 'react';
import { render, screen, fireEvent, waitFor } from '@testing-library/react';
import { MemoryRouter } from 'react-router-dom';
import UpdatePatientProfilePage from '../../../../Pages/Admin/PatientProfile/UpdatePatientProfilePage';
import fetchApi from '../../../../Services/fetchApi';

jest.mock('../../../../Services/fetchApi');

beforeEach(() => {
  fetchApi.mockClear();
  window.confirm = jest.fn().mockImplementation(() => true);
  window.alert = jest.fn();
});

test('renders UpdatePatientProfilePage and updates data', async () => {
  fetchApi.mockResolvedValueOnce([{
    firstName: 'John',
    lastName: 'Doe',
    fullName: 'John Doe',
    email: 'john.doe@example.com',
    contactInformation: '123456789',
    dateOfBirth: '1990-01-01T00:00:00.000Z',
    medicalRecordNumber: '12345'
  }]);

  fetchApi.mockResolvedValueOnce({ message: 'Patient profile updated successfully.' });

  render(
    <MemoryRouter>
      <UpdatePatientProfilePage />
    </MemoryRouter>
  );

  const selectInput = screen.getByRole('combobox');
  const searchButton = screen.getByRole('button', { name: /search/i });
  const updateButton = screen.getByRole('button', { name: /update/i });

  fireEvent.change(selectInput, { target: { value: 'fullName' } });
  const fullNameInputs = screen.getAllByPlaceholderText('Full Name');
  fireEvent.change(fullNameInputs[0], { target: { value: 'John Doe' } });

  fireEvent.click(searchButton);

  await waitFor(() => {
    expect(fetchApi).toHaveBeenCalledWith('/PatientProfile/all?fullName=John+Doe', {
      method: 'GET',
    });
  });

  await waitFor(() => {
    expect(screen.getByDisplayValue('John')).toBeInTheDocument();
    expect(screen.getByDisplayValue('Doe')).toBeInTheDocument();
    expect(screen.getByDisplayValue('john.doe@example.com')).toBeInTheDocument();
    expect(screen.getByDisplayValue('123456789')).toBeInTheDocument();
    expect(screen.getByDisplayValue('1990-01-01')).toBeInTheDocument();
    expect(screen.getByDisplayValue('12345')).toBeInTheDocument();
  });

  fireEvent.change(screen.getByPlaceholderText('First Name'), { target: { value: 'Jane' } });

  fireEvent.click(updateButton);

  expect(window.confirm).toHaveBeenCalledWith('Are you sure you want to update the profile of John Doe?');

  await waitFor(() => {
    expect(fetchApi).toHaveBeenCalledWith('/PatientProfile/update?fullName=John+Doe', {
      method: 'PUT',
      body: JSON.stringify({
        firstName: 'Jane',
        lastName: 'Doe',
        fullName: 'John Doe',
        email: 'john.doe@example.com',
        contactInformation: '123456789',
        dateOfBirth: '1990-01-01',
        medicalRecordNumber: '12345'
      }),
    });
  });

  await waitFor(() => {
    expect(window.alert).toHaveBeenCalledWith('Patient profile updated successfully. The patient will be informed about this change.');
  });
});

test('displays error message when no patient profile is found', async () => {
  fetchApi.mockResolvedValueOnce([]);

  render(
    <MemoryRouter>
      <UpdatePatientProfilePage />
    </MemoryRouter>
  );

  const selectInput = screen.getByRole('combobox');
  const searchButton = screen.getByRole('button', { name: /search/i });

  fireEvent.change(selectInput, { target: { value: 'fullName' } });
  const fullNameInputs = screen.getAllByPlaceholderText('Full Name');
  fireEvent.change(fullNameInputs[0], { target: { value: 'Nonexistent Name' } });

  fireEvent.click(searchButton);

  await waitFor(() => {
    expect(fetchApi).toHaveBeenCalledWith('/PatientProfile/all?fullName=Nonexistent+Name', {
      method: 'GET',
    });
  });

  await waitFor(() => {
    expect(screen.getByText('No patient profile found.')).toBeInTheDocument();
  });
});

test('displays error message when update fails', async () => {
  fetchApi.mockResolvedValueOnce([{
    firstName: 'John',
    lastName: 'Doe',
    fullName: 'John Doe',
    email: 'john.doe@example.com',
    contactInformation: '123456789',
    dateOfBirth: '1990-01-01T00:00:00.000Z',
    medicalRecordNumber: '12345'
  }]);

  fetchApi.mockRejectedValueOnce(new Error('Update failed'));

  render(
    <MemoryRouter>
      <UpdatePatientProfilePage />
    </MemoryRouter>
  );

  const selectInput = screen.getByRole('combobox');
  const searchButton = screen.getByRole('button', { name: /search/i });
  const updateButton = screen.getByRole('button', { name: /update/i });

  fireEvent.change(selectInput, { target: { value: 'fullName' } });
  const fullNameInputs = screen.getAllByPlaceholderText('Full Name');
  fireEvent.change(fullNameInputs[0], { target: { value: 'John Doe' } });

  fireEvent.click(searchButton);

  await waitFor(() => {
    expect(fetchApi).toHaveBeenCalledWith('/PatientProfile/all?fullName=John+Doe', {
      method: 'GET',
    });
  });

  await waitFor(() => {
    expect(screen.getByDisplayValue('John')).toBeInTheDocument();
    expect(screen.getByDisplayValue('Doe')).toBeInTheDocument();
    expect(screen.getByDisplayValue('john.doe@example.com')).toBeInTheDocument();
    expect(screen.getByDisplayValue('123456789')).toBeInTheDocument();
    expect(screen.getByDisplayValue('1990-01-01')).toBeInTheDocument();
    expect(screen.getByDisplayValue('12345')).toBeInTheDocument();
  });

  fireEvent.change(screen.getByPlaceholderText('First Name'), { target: { value: 'Jane' } });

  fireEvent.click(updateButton);

  expect(window.confirm).toHaveBeenCalledWith('Are you sure you want to update the profile of John Doe?');

  await waitFor(() => {
    expect(fetchApi).toHaveBeenCalledWith('/PatientProfile/update?fullName=John+Doe', {
      method: 'PUT',
      body: JSON.stringify({
        firstName: 'Jane',
        lastName: 'Doe',
        fullName: 'John Doe',
        email: 'john.doe@example.com',
        contactInformation: '123456789',
        dateOfBirth: '1990-01-01',
        medicalRecordNumber: '12345'
      }),
    });
  });

  await waitFor(() => {
    expect(screen.getByText('Update failed')).toBeInTheDocument();
  });
});