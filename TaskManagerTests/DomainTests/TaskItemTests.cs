
using TaskManager.Domaine.Entities;
using TaskManager.Domaine.ValueObjects;

namespace TaskManagerTests.DomainTests
{
    [TestFixture]
    public class TaskItemTests
    {
        [Test]
        public void TaskItem_Should_Have_Title_And_NotCompleted()
        {
            var title = new TaskTitle("My Task");
            var task = new TaskItem(title);

            Assert.That(task.Title, Is.EqualTo(title));
            Assert.That(task.IsCompleted, Is.False);
        }

        [Test]
        public void MarkComplete_Should_Set_IsCompleted_To_True()
        {
            var task = new TaskItem(new TaskTitle("Test"));

            task.Complete();

            Assert.That(task.IsCompleted, Is.True);
        }

        [Test]
        public void MarkComplete_Should_Throw_When_AlreadyCompleted()
        {
            var task = new TaskItem(new TaskTitle("Test"));
            task.Complete();

            var ex = Assert.Throws<InvalidOperationException>(() => task.Complete());
            Assert.That(ex.Message, Is.EqualTo("Task already completed."));
        }
    }
}
