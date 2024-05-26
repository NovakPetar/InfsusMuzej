using NUnit.Framework;
using Moq;
using Muzej.DAL.Models;
using Muzej.Repository.Interfaces;
using Muzej.SqlServerRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Muzej.SqlServerRepository.Tests
{
    [TestFixture]
    public class JobsRepositoryTests
    {
        private IJobsRepository _repository;
        private Mock<MUZContext> _mockContext;
        private List<Job> _jobs;

        [SetUp]
        public void Setup()
        {
            _jobs = new List<Job>
            {
                new Job { JobId = 1, Name = "Manager" },
                new Job { JobId = 2, Name = "Assistant" }
            };

            _mockContext = new Mock<MUZContext>();
            var mockSet = CreateMockJobDbSet(_jobs);

            _mockContext.Setup(m => m.Jobs).Returns(mockSet.Object);

            _repository = new JobsRepository(_mockContext.Object);
        }

        
        public static Mock<DbSet<Job>> CreateMockJobDbSet(List<Job> data)
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<Job>>();
            mockSet.As<IQueryable<Job>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<Job>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<Job>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<Job>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            mockSet.Setup(m => m.Add(It.IsAny<Job>())).Callback<Job>(entity =>
            {
                data.Add(entity);
            });
            mockSet.Setup(m => m.Remove(It.IsAny<Job>())).Callback<Job>(entity => data.Remove(entity));
            mockSet.Setup(m => m.Update(It.IsAny<Job>())).Callback<Job>(entity =>
            {
                var item = data.FirstOrDefault(i => i.JobId == entity.JobId);
                if (item != null)
                {
                    data.Remove(item);
                }
                data.Add(entity);
            });
            return mockSet;
        }

        [Test]
        public void GetJob_ExistingJob_ReturnsJob()
        {
            int existingJobId = 1;

            var job = _repository.GetJob(existingJobId);

            Assert.IsNotNull(job);
            Assert.AreEqual(existingJobId, job.JobId);
        }

        [Test]
        public void GetJob_NonExistingJob_ReturnsNull()
        {
            int nonExistingJobId = 100;

            var job = _repository.GetJob(nonExistingJobId);

            Assert.IsNull(job);
        }

        [Test]
        public void GetJobs_ReturnsJobs()
        {
            var jobs = _repository.GetJobs();

            Assert.IsNotNull(jobs);
            Assert.AreEqual(_jobs.Count, jobs.Count);
        }

        [Test]
        public void UpdateJob_ExistingJob_UpdatesJob()
        {
            var job = _repository.GetJob(1);
            job.Name = "UpdatedJobTitle";

            var result = _repository.UpdateJob(job);

            Assert.IsTrue(result);
            Assert.AreEqual("UpdatedJobTitle", _jobs.First(e => e.JobId == 1).Name);
        }

        [Test]
        public void CreateJob_NewJob_CreatesJob()
        {
            var newJob = new Muzej.DomainObjects.Job
            {
                Name = "New Job"
            };

            var jobId = _repository.CreateJob(newJob);
            
            Assert.IsTrue(_jobs.Any(e => e.JobId != jobId));
        }

        [Test]
        public void DeleteJob_ExistingJob_DeletesJob()
        {
            int existingJobId = 1;

            var result = _repository.DeleteJob(existingJobId);

            Assert.IsTrue(result);
            Assert.IsFalse(_jobs.Where(e => e.JobId == existingJobId).Count() != 0);
        }
    }
}
