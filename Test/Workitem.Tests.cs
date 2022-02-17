using Site.Pages;
using Site.Data;
using Site.Data.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bunit;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Test;

/// <summary>
/// These tests are written entirely in C#.
/// Learn more at https://bunit.dev/docs/getting-started/writing-tests.html#creating-basic-tests-in-cs-files
/// </summary>
public class CounterCSharpTests : BunitTestContext
{

	public CounterCSharpTests ()
	{
	}

	protected Bunit.TestContext GetContext()
	{
		var ctx = new Bunit.TestContext();
		ctx.Services.AddDbContextFactory<WorkitemContext>(opt =>
				opt.UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
			.EnableSensitiveDataLogging(), ServiceLifetime.Singleton);
		ctx.Services.AddSingleton<WorkitemContext>();
		ctx.Services.AddSingleton<WorkitemService>();
		ctx.Services.GetService<WorkitemContext>().Workitem.AddRange(new[]{
			new Workitem{ ID = 1, Title = "Title 1", Description = "Description 1"},
			new Workitem{ ID = 2, Title = "Title 2", Description = "Description 2"}
		});
		return ctx;
	}

	[Test]
	[Timeout(4000)]
	public async Task AddingWorkitem()
	{
		using (var ctx = GetContext())
		{
			var wIcontext = ctx.Services.GetService<WorkitemContext>();
			var workitems = await wIcontext.Workitem.ToListAsync();
			Assert.False(workitems.Exists(x => x.ID == 123));

			var cut = ctx.RenderComponent<Wi>();
			cut.WaitForElement("form", TimeSpan.FromSeconds(1));

			cut.Find("input[id=ID]").Change(123);
			cut.Find("input[id=Title]").Change("Awesome Title");
			cut.Find("textarea[id=Description]").Change("Awesome Description");
			cut.Find("button[type=submit]").Click();

			cut.WaitForElements("tr[key=123]", TimeSpan.FromSeconds(2));
		}
	}


	[Test]
	[Timeout(10000)]
	public async Task AddingWorkitems_FrontEnd()
	{
		using (var ctx = GetContext())
		{
			var wIcontext = ctx.Services.GetService<WorkitemContext>();
			var workitems = await wIcontext.Workitem.ToListAsync();
			Assert.False(workitems.Exists(x => x.ID == 1002));
			Assert.False(workitems.Exists(x => x.ID == 1003));

			var cut = ctx.RenderComponent<Wi>();
			
			cut.WaitForElement("form", TimeSpan.FromSeconds(1));
			cut.Find("input[id=ID]").Change(1002);
			cut.Find("input[id=Title]").Change("Awesome Title 1002");
			cut.Find("textarea[id=Description]").Change("Awesome Description 1002");
			cut.Find("button[type=submit]").Click();
			cut.WaitForElement("tr[key=1002]", TimeSpan.FromSeconds(1));

			cut.WaitForElement("form", TimeSpan.FromSeconds(1));
			cut.Find("input[id=ID]").Change(1003);
			cut.Find("input[id=Title]").Change("Awesome Title 1003");
			cut.Find("textarea[id=Description]").Change("Awesome Description 1003");
			cut.Find("button[type=submit]").Click();
			cut.WaitForElement("tr[key=1002],tr[key=1003]", TimeSpan.FromSeconds(1));

			var tr1002 = cut.Find("tr[key=1002]");
			Assert.AreEqual(tr1002.InnerHtml, "<td>1002</td>\n          <td>Awesome Title 1002</td>\n          <td>Awesome Description 1002</td>");
			var tr1003 = cut.Find("tr[key=1003]");
			Assert.AreEqual(tr1003.InnerHtml, "<td>1003</td>\n          <td>Awesome Title 1003</td>\n          <td>Awesome Description 1003</td>");
		}
	}


	[Test]
	[Timeout(4000)]
	public async Task AddingWorkitems_BackEnd()
	{
		using (var ctx = GetContext())
		{
			var wIcontext = ctx.Services.GetService<WorkitemContext>();
			var workitems = await wIcontext.Workitem.ToListAsync();
			Assert.False(workitems.Exists(x => x.ID == 1002));
			Assert.False(workitems.Exists(x => x.ID == 1003));

			var dummy = new Workitem
			{
				ID = 2002,
				Title = "Title 2002",
				Description = "Description 2002",
				Creation = DateTime.UtcNow,
				LastChange = DateTime.UtcNow
			};

			await ctx.Services.GetService<WorkitemService>().InsertWorkitem(dummy);

			workitems = await wIcontext.Workitem.ToListAsync();
			var workitem = workitems.Find(x => x.ID == dummy.ID);
			Assert.NotNull(workitem);
			Assert.AreEqual(workitem.Title, dummy.Title);
			Assert.AreEqual(workitem.Description, dummy.Description);
			Assert.AreEqual(workitem.Creation, dummy.Creation);
		}
	}
}
