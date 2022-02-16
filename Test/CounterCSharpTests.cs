using Site.Pages;
using Site.Data;
using Site.Data.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Test;

/// <summary>
/// These tests are written entirely in C#.
/// Learn more at https://bunit.dev/docs/getting-started/writing-tests.html#creating-basic-tests-in-cs-files
/// </summary>
public class CounterCSharpTests : BunitTestContext
{
	private Bunit.TestContext ctx = new();

	public CounterCSharpTests ()
	{
		ctx.Services.AddSingleton<WorkitemContext>();
		ctx.Services.AddSingleton<WorkitemService>();
	}

    [Test]
	[Timeout(4000)]
	public async Task AddingWorkitem()
	{
		var workitems = await ctx.Services.GetService<WorkitemContext>().GetWorkitems();
		Assert.False(workitems.Exists(x => x.ID == 123));

		var cut = ctx.RenderComponent<Wi>();

		cut.Find("input[form=\"ID\"]").Change(123);
		cut.Find("input[form=\"Title\"]").Change("Awesome Title");
		cut.Find("input[form=\"Description\"]").Change("Awesome Description");
		cut.Find("input[type=\"button\"]").Click();

		workitems = await ctx.Services.GetService<WorkitemContext>().GetWorkitems();
		var workitem = workitems.Find(x => x.ID == 123);
		Assert.NotNull(workitem);
		Assert.AreEqual(workitem.ID, 123);
		Assert.AreEqual(workitem.Title, "Awesome Title");
		Assert.AreEqual(workitem.Description, "Awesome Description");
	}

	[Test]
	[Timeout(4000)]
	public async Task AddingWorkitem2()
	{
		var workitems = await ctx.Services.GetService<WorkitemContext>().GetWorkitems();
		Assert.False(workitems.Exists(x => x.ID == 234));

		var cut = ctx.RenderComponent<Wi>();

		cut.Find("input[form=\"ID\"]").Change(234);
		cut.Find("input[form=\"Title\"]").Change("Awesome Title 2");
		cut.Find("input[form=\"Description\"]").Change("Awesome Description 2");
		cut.Find("input[type=\"button\"]").Click();

		workitems = await ctx.Services.GetService<WorkitemContext>().GetWorkitems();
		var workitem = workitems.Find(x => x.ID == 234);
		Assert.NotNull(workitem);
		Assert.AreEqual(workitem.ID, 234);
		Assert.AreEqual(workitem.Title, "Awesome Title 2");
		Assert.AreEqual(workitem.Description, "Awesome Description 2");
	}
}
