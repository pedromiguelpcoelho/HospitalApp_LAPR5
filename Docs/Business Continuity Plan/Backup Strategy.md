# Backup Strategy for Minimizing RPO and WRT

## 1. Introduction
As a system administrator, it is crucial to have a robust backup strategy that minimizes the Recovery Point Objective (RPO) and Work Recovery Time (WRT). This document outlines a proposed backup strategy for the SEM5_PI_GRUPO44 project, justifies the chosen methods, and provides implementation details to ensure data integrity and availability.

## 2. Objectives
- **Minimize RPO**: Ensure that the maximum acceptable amount of data loss measured in time is minimized.
- **Minimize WRT**: Ensure that the time required to recover and resume normal operations after a failure is minimized.

## 3. Backup Strategy
### 3.1. Backup Types
#### Full Backups:
- **Description**: A complete copy of all data.
- **Frequency**: Weekly.
- **Justification**: Provides a comprehensive restore point, ensuring that all data can be recovered in the event of a major failure.

#### Incremental Backups:
- **Description**: Only the data that has changed since the last backup (full or incremental) is backed up.
- **Frequency**: Daily.
- **Justification**: Reduces the amount of data to be backed up daily, saving storage space and reducing backup time.

### 3.2. Backup Storage
#### On-Premises Storage:
- **Description**: All backups will be stored on the VM at the DEI.
- **Justification**: Using the VM at the DEI provides both fast access to backups for quick recovery and reduces dependency on external storage solutions. It also keeps the backup process fully contained within the DEI infrastructure.
  

### 3.3. Backup Schedule
| Backup Type          | Frequency         | Storage Location          |
|---------------------|------------------|---------------------------|
| Full Backup          | Weekly           | VM at DEI                |
| Incremental Backup   | Daily            | VM at DEI                |

## 4. Implementation
### 4.1. Tools and Technologies
#### Database Backups:
- **Tool**: BCP (Bulk Copy Program) for SQL Server databases.
- **Justification**: BCP is used for fast and efficient bulk copying of data between SQL Server and data files, making it ideal for performing incremental backups.
- **Script**:  Automated scripts to perform full and incremental backups of the database using BCP.

### 4.2. Backup Verification

#### Manual Verification:
- **Description**: Periodic manual checks to ensure backups are complete and recoverable.
- **Procedure**: Perform test restores and verify data integrity.

### 4.3. Recovery Procedures
#### Database Recovery:
- **Procedure**: Use automated scripts to restore the latest full backup followed by the latest incremental backups.


## 5. Justification
- **Minimizing RPO**: The combination of full, incremental, and differential backups ensures that data loss is minimized by providing multiple restore points.
- **Minimizing WRT**: The use of on-premises storage for fast access and cloud storage for disaster recovery ensures that recovery times are minimized.
- **Cost-Effectiveness**: The use of incremental and differential backups reduces storage costs and backup times compared to full backups alone.

## 6. Conclusion
Implementing this backup strategy will ensure that the SEM5_PI_GRUPO44 project has a robust and reliable backup system in place, minimizing both RPO and WRT. Regular verification and testing of backups will further ensure data integrity and availability in the event of a failure.