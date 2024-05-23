using NUnit.Framework;
using Moq;
using Muzej.BLL;
using Muzej.DomainObjects;
using Muzej.Repository.Interfaces;
using System.Collections.Generic;

namespace Muzej.BLL.Tests
{
    [TestFixture]
    public class JobsBLLTest
    {
        private JobsBLL _jobsBLL;
        private Mock<IJobsRepository> _mockJobsRepository;
        private Mock<IRepositoryWrapper> _mockRepositoryWrapper;

        [SetUp]
        public void Setup()
        {
            _mockJobsRepository = new Mock<IJobsRepository>();
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            
            _mockRepositoryWrapper.Setup(r => r.Jobs).Returns(_mockJobsRepository.Object);
            
            _jobsBLL = new JobsBLL(_mockRepositoryWrapper.Object);
        }

        [Test]
        public void GetJob_ShouldReturnJob_WhenJobExists()
        {
            var jobId = 1;
            var expectedJob = new Job { JobId = jobId };
            _mockJobsRepository.Setup(r => r.GetJob(jobId)).Returns(expectedJob);
            
            var result = _jobsBLL.GetJob(jobId);
            
            Assert.AreEqual(expectedJob, result);
        }

        [Test]
        public void GetJobsForEmployee_ShouldReturnJobs_WhenJobsExist()
        {
            var employeeId = 1;
            var expectedJobs = new List<Job> { new Job { JobId = 1 }, new Job { JobId = 2 } };
            _mockJobsRepository.Setup(r => r.GetJobs()).Returns(expectedJobs);

            var result = _jobsBLL.GetJobs();
            
            Assert.AreEqual(expectedJobs, result);
        }

        [Test]
        public void UpdateJob_ShouldReturnTrue_WhenJobIsUpdated()
        {
            var job = new Job { JobId = 1 };
            _mockJobsRepository.Setup(r => r.UpdateJob(job)).Returns(true);
            
            var result = _jobsBLL.UpdateJob(job);
            
            Assert.IsTrue(result);
        }

        [Test]
        public void CreateJob_ShouldReturnJobId_WhenJobIsCreated()
        {
            var job = new Job { JobId = 1 };
            var expectedJobId = 1;
            _mockJobsRepository.Setup(r => r.CreateJob(job)).Returns(expectedJobId);
            
            var result = _jobsBLL.CreateJob(job);
            
            Assert.AreEqual(expectedJobId, result);
        }

        [Test]
        public void DeleteJob_ShouldReturnTrue_WhenJobIsDeleted()
        {
            var jobId = 1;
            _mockJobsRepository.Setup(r => r.DeleteJob(jobId)).Returns(true);
            
            var result = _jobsBLL.DeleteJob(jobId);
            
            Assert.IsTrue(result);
        }
    }
}
