using FluentAssertions;
using SmartHospital.Letters.Entities;
using SmartHospital.Letters.UseCases.UpdateLetter;
using Xunit;

namespace SmartHospital.Letters.UseCases.Tests;

public class UpdateLetterHandlerTests
{
	[Fact]
	public void RemoveDeleted_ShouldRemoveDeletedItems()
	{
		// Arrange
		var existing = Enumerable.Range(0, 5)
			.Select(e => new GuidIdentifier(Guid.NewGuid()))
			.ToList();
		var incoming = existing.Take(3).ToList();

		// Act
		UpdateLetterHandler.RemoveDeleted(existing, incoming);

		// Assert
		existing.Count.Should().Be(2);
	}

	private record GuidIdentifier(Guid Id) : IIdentifier<Guid>;
}
