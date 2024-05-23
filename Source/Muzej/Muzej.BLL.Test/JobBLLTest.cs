// File: Muzej.Tests/JobsBLLTests.cs
using NUnit.Framework;
using Moq;
using Muzej.BLL;
using Muzej.DomainObjects;
using Muzej.Repository.Interfaces;
using System.Collections.Generic;

namespace Muzej.Tests
{
    [TestFixture]
    public class JobsBLLTests
    {
        private JobsBLL _jobsBLL;
        private Mock<IJobsRepository> _mockJobsRepository;
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;

        [SetUp]
        public void Setup()
        {
            // Initialize the mocks
            _mockJobsRepository = new Mock<IJobsRepository>();
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();

            // Setup the mock behavior
            _mockRepositoryWrapper.Setup(r => r.Jobs).Returns(_mockJobsRepository.Object);

            // Initialize the JobsBLL with the mocked repository wrapper
            _jobsBLL = new JobsBLL(_mockRepositoryWrapper.Object);
        }

        [Test]
        public void GetJob_ShouldReturnJob_WhenJobExists()
        {
            // Arrange
            var jobId = 1;
            var expectedJob = new Job { JobId = jobId };
            _mockJobsRepository.Setup(r => r.GetJob(jobId)).Returns(expectedJob);

            // Act
            var result = _jobsBLL.GetJob(jobId);

            // Assert
            Assert.AreEqual(expectedJob, result);
        }

        [Test]
        public void GetJobsForEmployee_ShouldReturnJobs_WhenJobsExist()
        {
            // Arrange
            var employeeId = 1;
            var expectedJobs = new List<Job> { new Job { JobId = 1 }, new Job { JobId = 2 } };
            _mockJobsRepository.Setup(r => r.GetJobs()).Returns(expectedJobs);

            // Act
            var result = _jobsBLL.GetJobs();

            // Assert
            Assert.AreEqual(expectedJobs, result);
        }

        [Test]
        public void UpdateJob_ShouldReturnTrue_WhenJobIsUpdated()
        {
            // Arrange
            var job = new Job { JobId = 1 };
            _mockJobsRepository.Setup(r => r.UpdateJob(job)).Returns(true);

            // Act
            var result = _jobsBLL.UpdateJob(job);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CreateJob_ShouldReturnJobId_WhenJobIsCreated()
        {
            // Arrange
            var job = new Job { JobId = 1 };
            var expectedJobId = 1;
            _mockJobsRepository.Setup(r => r.CreateJob(job)).Returns(expectedJobId);

            // Act
            var result = _jobsBLL.CreateJob(job);

            // Assert
            Assert.AreEqual(expectedJobId, result);
        }

        [Test]
        public void DeleteJob_ShouldReturnTrue_WhenJobIsDeleted()
        {
            // Arrange
            var jobId = 1;
            _mockJobsRepository.Setup(r => r.DeleteJob(jobId)).Returns(true);

            // Act
            var result = _jobsBLL.DeleteJob(jobId);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
