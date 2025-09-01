using TaskManager.Domaine.ValueObjects;

namespace TaskManagerTests.DomainTests
{
    [TestFixture]
    public class TaskTitleTests
    {
        [Test]
        public void Constructor_Should_Set_Value_When_Valid()
        {
            var title = new TaskTitle("My Task");
            Assert.That(title.Value, Is.EqualTo("My Task"));
        }
    }
}
