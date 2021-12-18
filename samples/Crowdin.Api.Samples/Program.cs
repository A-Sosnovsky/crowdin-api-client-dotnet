﻿
// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using Crowdin.Api;
using Crowdin.Api.ProjectsGroups;

var client = new CrowdinApiClient(new CrowdinCredentials
{
    AccessToken = "<paste token here>",
    Organization = "optional organization (for Enterprise API)"
});

ResponseList<EnterpriseProject> response = await client.ProjectsGroups.ListProjects<EnterpriseProject>();

const int projectId = 1;

var operations = new List<ProjectPatch>
{
    new ProjectInfoPatch
    {
        Value = 1,
        Path = ProjectInfoPathCode.Cname,
        Operation = PatchOperation.Replace
    },
    new ProjectInfoPatch
    {
        Value = "test",
        Path = new ProjectInfoPath(ProjectInfoPathCode.LanguageMapping, "en", "2"),
        Operation = PatchOperation.Test
    },
    new ProjectSettingPatch
    {
        Value = true,
        Path = ProjectSettingPathCode.AutoSubstitution,
        Operation = PatchOperation.Replace
    }
};

var projectSettingsResponse = await client.ProjectsGroups.EditProject<ProjectSettings>(projectId, operations);
Console.WriteLine(projectSettingsResponse);