using Am.I.Online.Core.Utils;
using FluentAssertions;
using NUnit.Framework;

namespace Am.I.Online.Core.Tests.Utils
{
  public class ExampleTests
  {
    private Example? _example;

    private void Setup()
    {
      _example = new Example();
    }

    [Test]
    public void Valid_WhenCalled_ShouldReturnTrue()
    {
      // arrange
      Setup();
      // action
      var valid = _example!.Valid();
      // assert
      valid.Should().Be(true);
    }
  }
}
