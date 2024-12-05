module.exports = {
    setupFilesAfterEnv: ['<rootDir>/src/setupTests.js'],
    testEnvironment: 'jsdom',
  };

// jest.config.js
module.exports = {
  setupFilesAfterEnv: ['./jest.setup.js'],
};