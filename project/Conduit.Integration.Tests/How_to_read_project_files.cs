using System;
using Xunit;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Utilities;
using System.Xml;
using System.IO;
using System.Text;

namespace Conduit.Integration.Tests
{
	public class How_to_read_project_files
	{
		public How_to_read_project_files ()
		{
			
		}

		[Fact]
		public void for_example() {
			string projectFileXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  					<Target Name=""example"">
  					</Target>
				</Project>";

			using (var reader = XmlReader.Create(new StringReader(projectFileXml)))
			{
				var project = new Project(reader);

				Assert.True(project.Targets.Count == 1, "Expected exactly one target because the file does not import any and declares one");

				Assert.True (project.Targets.ContainsKey("example"), "Expected to have read the file and to've found the task called 'example'");
			}	
		}

		// TEST: It would be nice to be able to ignore imported tasks
	}
}

