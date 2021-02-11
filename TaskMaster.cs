using System;
using System.Collections.Generic;
using TaskManagementDLL.Scripts.ContractorManagement;
using TaskManagementDLL.Scripts.ProjectManagement;

namespace TaskManagementDLL
{
    public class TaskMaster
    {
        private List<Project> _projects = new List<Project>();
        private List<Contractor> _contractors = new List<Contractor>();

        public TaskMaster()
        {
        }

        public List<Project> Projects => _projects;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="start"></param>
        /// <param name="deadline"></param>
        /// <exception cref="ArgumentException"></exception>
        public void CreateProject(string name, DateTime start, DateTime deadline)
        {
            if (Projects.TrueForAll(project => project.Name != name))
                Projects.Add(new Project(name, start, deadline));
            else
                throw new ArgumentException($"Project with name {name} already exists");
        }

        public void CloseProject(string name)
        {
            var project = Projects.Find(pr => pr.Name == name);
            project.Close();
            Projects.Remove(project);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="previousName"></param>
        /// <param name="newName"></param>
        /// <exception cref="ArgumentException"></exception>
        public void ChangeProjectName(string previousName, string newName)
        {
            foreach (var project in Projects)
            {
                if (project.Name == previousName)
                {
                    project.Name = newName;
                    return;
                }
            }
            throw new ArgumentException($"No project with name {previousName} exists");
        }
    }
}
