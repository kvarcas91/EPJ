﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace EPJ.ViewModels
{
    public class ProjectListViewModel : Screen
    {

        #region Constructors
        
        public ProjectListViewModel()
        {
            //for (int i = 0; i < 100; i++)
            //{
                AddFakeData();
            //}

            //mAddProjectCommand = new RelayCommand(ShowMessage);
            ShowListItemSettingsCommand = new RelayCommand(ShowListItemSettings);
            ShowProjectCommand = new RelayCommand(ShowProject);
        }


        #endregion

        #region Properties

       // public ICommand mAddProjectCommand { get; set; }

       public ICommand ShowListItemSettingsCommand { get; set; }

        public ICommand ShowProjectCommand { get; set; }


        /// <summary>
        /// List of all projects
        /// </summary>
        public BindableCollection<Project> Projects { get; private set; } 


        public List<string> SortBy
        {
            get
            {
                return EnumHelper.SortByEnumToString;
            }
        }

        #endregion

       public void LoadAddProjectPage ()
        {
            //MessageBox.Show("test");
            AddProjectViewModel lg = new AddProjectViewModel();
            var parentConductor = (Conductor<object>)(this.Parent);
            parentConductor.ActivateItem(lg);
            
        }

        public void ShowListItemSettings (object param)
        {
            MessageBox.Show("Labas");
            var project = (IProject)param;
        }

        public void ShowProject (object param)
        {
            var project = (IProject)param;
            MessageBox.Show($"project name: {project.Title}");
        }

        public void AddFakeData ()
        {
            //List<Contributor> contributors = DataBase.GetContributors();
            Projects = new BindableCollection<Project>(DataBase.GetProjects());

          
            /*
            var mProject = new Project(0);
            mProject.AddContributor(contributors[0])
                    .AddContributor(contributors[1])
                    .AddComment("First comment");
            mProject.Priority = Priority.HIGH;
            mProject.Name = "Pirmas projektas";
            mProject.Description = "Woody equal ask saw sir weeks aware decay. Entrance prospect removing we packages strictly is no smallest he. For hopes may chief get hours day rooms. Oh no turned behind polite piqued enough at. Forbade few through inquiry blushes you. Cousin no itself eldest it in dinner latter missed no. Boisterous estimating interested collecting get conviction friendship say boy. Him mrs shy article smiling respect opinion excited. Welcomed humoured rejoiced peculiar to in an. " +
                                    "Building mr concerns servants in he outlived am breeding. He so lain good miss when sell some at if.Told hand so an rich gave next. How doubt yet again see son smart.While mirth large of on front. Ye he greater related adapted proceed entered an. Through it examine express promise no. Past add size game cold girl off how old." +
                                    "Wrong do point avoid by fruit learn or in death.So passage however besides invited comfort elderly be me.Walls began of child civil am heard hoped my.Satisfied pretended mr on do determine by. Old post took and ask seen fact rich.Man entrance settling believed eat joy. Money as drift begin on to.Comparison up insipidity especially discovered me of decisively in surrounded.Points six way enough she its father.Folly sex downs tears ham green forty. ";
           
            mProject.Progress = 10;

            var mProject2 = new Project(0);
            mProject2.AddContributor(contributors[2])
                    .AddComment("First comment");
            mProject2.Priority = Priority.MEDIUM;
            mProject2.Name = "Antras projektelis";
            mProject2.Progress = 93;
            mProject2.Description = "Woody equal ask saw sir weeks aware decay. Entrance prospect removing we packages strictly is no smallest he. For hopes may chief get hours day rooms. Oh no turned behind polite piqued enough at. Forbade few through inquiry blushes you. Cousin no itself eldest it in dinner latter missed no. Boisterous estimating interested collecting get conviction friendship say boy. Him mrs shy article smiling respect opinion excited. Welcomed humoured rejoiced peculiar to in an. " +
                                   "Building mr concerns servants in he outlived am breeding. He so lain good miss when sell some at if.Told hand so an rich gave next. How doubt yet again see son smart.While mirth large of on front. Ye he greater related adapted proceed entered an. Through it examine express promise no. Past add size game cold girl off how old." +
                                   "Wrong do point avoid by fruit learn or in death.So passage however besides invited comfort elderly be me.Walls began of child civil am heard hoped my.Satisfied pretended mr on do determine by. Old post took and ask seen fact rich.Man entrance settling believed eat joy. Money as drift begin on to.Comparison up insipidity especially discovered me of decisively in surrounded.Points six way enough she its father.Folly sex downs tears ham green forty. ";

            var mProject3 = new Project(0);
            mProject3.AddContributor(contributors[3])
                    .AddContributor(contributors[4])
                     .AddContributor(contributors[5])
                    .AddComment("First comment");
            mProject3.Priority = Priority.LOW;
            mProject3.Name = "Test project";
            mProject3.Progress = 49;
            mProject3.Description = "Woody equal ask saw sir weeks aware decay. Entrance prospect removing we packages strictly is no smallest he. For hopes may chief get hours day rooms. Oh no turned behind polite piqued enough at. Forbade few through inquiry blushes you. Cousin no itself eldest it in dinner latter missed no. Boisterous estimating interested collecting get conviction friendship say boy. Him mrs shy article smiling respect opinion excited. Welcomed humoured rejoiced peculiar to in an. " +
                                   "Building mr concerns servants in he outlived am breeding. He so lain good miss when sell some at if.Told hand so an rich gave next. How doubt yet again see son smart.While mirth large of on front. Ye he greater related adapted proceed entered an. Through it examine express promise no. Past add size game cold girl off how old." +
                                   "Wrong do point avoid by fruit learn or in death.So passage however besides invited comfort elderly be me.Walls began of child civil am heard hoped my.Satisfied pretended mr on do determine by. Old post took and ask seen fact rich.Man entrance settling believed eat joy. Money as drift begin on to.Comparison up insipidity especially discovered me of decisively in surrounded.Points six way enough she its father.Folly sex downs tears ham green forty. ";


            var mProject4 = new Project(0);
            mProject4.AddContributor(contributors[3])
                    .AddContributor(contributors[4])
                     .AddContributor(contributors[5])
                    .AddComment("First comment");
            mProject4.Priority = Priority.LOW;
            mProject4.Name = "Test project";
            mProject4.Progress = 49;
            mProject4.Description = "Woody equal ask saw sir weeks aware decay. Entrance prospect removing we packages strictly is no smallest he. For hopes may chief get hours day rooms. Oh no turned behind polite piqued enough at. Forbade few through inquiry blushes you. Cousin no itself eldest it in dinner latter missed no. Boisterous estimating interested collecting get conviction friendship say boy. Him mrs shy article smiling respect opinion excited. Welcomed humoured rejoiced peculiar to in an. " +
                                   "Building mr concerns servants in he outlived am breeding. He so lain good miss when sell some at if.Told hand so an rich gave next. How doubt yet again see son smart.While mirth large of on front. Ye he greater related adapted proceed entered an. Through it examine express promise no. Past add size game cold girl off how old." +
                                   "Wrong do point avoid by fruit learn or in death.So passage however besides invited comfort elderly be me.Walls began of child civil am heard hoped my.Satisfied pretended mr on do determine by. Old post took and ask seen fact rich.Man entrance settling believed eat joy. Money as drift begin on to.Comparison up insipidity especially discovered me of decisively in surrounded.Points six way enough she its father.Folly sex downs tears ham green forty. ";


            var mProject5 = new Project(0);
            mProject5.AddContributor(contributors[2])
                    .AddContributor(contributors[2])
                     .AddContributor(contributors[2])
                    .AddComment("First comment");
            mProject5.Priority = Priority.LOW;
            mProject5.Name = "Test project";
            mProject5.Progress = 49;
            mProject5.Description = "Woody equal ask saw sir weeks aware decay. Entrance prospect removing we packages strictly is no smallest he. For hopes may chief get hours day rooms. Oh no turned behind polite piqued enough at. Forbade few through inquiry blushes you. Cousin no itself eldest it in dinner latter missed no. Boisterous estimating interested collecting get conviction friendship say boy. Him mrs shy article smiling respect opinion excited. Welcomed humoured rejoiced peculiar to in an. " +
                                   "Building mr concerns servants in he outlived am breeding. He so lain good miss when sell some at if.Told hand so an rich gave next. How doubt yet again see son smart.While mirth large of on front. Ye he greater related adapted proceed entered an. Through it examine express promise no. Past add size game cold girl off how old." +
                                   "Wrong do point avoid by fruit learn or in death.So passage however besides invited comfort elderly be me.Walls began of child civil am heard hoped my.Satisfied pretended mr on do determine by. Old post took and ask seen fact rich.Man entrance settling believed eat joy. Money as drift begin on to.Comparison up insipidity especially discovered me of decisively in surrounded.Points six way enough she its father.Folly sex downs tears ham green forty. ";


            var mProject6 = new Project(0);
            mProject6.AddContributor(contributors[2])
                    .AddContributor(contributors[2])
                     .AddContributor(contributors[2])
                    .AddComment("First comment");
            mProject6.Priority = Priority.HIGH;
            mProject6.Name = "Test project";
            mProject6.Progress = 49;
            mProject6.Description = "Woody equal ask saw sir weeks aware decay. Entrance prospect removing we packages strictly is no smallest he. For hopes may chief get hours day rooms. Oh no turned behind polite piqued enough at. Forbade few through inquiry blushes you. Cousin no itself eldest it in dinner latter missed no. Boisterous estimating interested collecting get conviction friendship say boy. Him mrs shy article smiling respect opinion excited. Welcomed humoured rejoiced peculiar to in an. " +
                                   "Building mr concerns servants in he outlived am breeding. He so lain good miss when sell some at if.Told hand so an rich gave next. How doubt yet again see son smart.While mirth large of on front. Ye he greater related adapted proceed entered an. Through it examine express promise no. Past add size game cold girl off how old." +
                                   "Wrong do point avoid by fruit learn or in death.So passage however besides invited comfort elderly be me.Walls began of child civil am heard hoped my.Satisfied pretended mr on do determine by. Old post took and ask seen fact rich.Man entrance settling believed eat joy. Money as drift begin on to.Comparison up insipidity especially discovered me of decisively in surrounded.Points six way enough she its father.Folly sex downs tears ham green forty. ";


            Projects.Add(mProject);
            Projects.Add(mProject2);
            Projects.Add(mProject3);
            Projects.Add(mProject4);
            Projects.Add(mProject5);
            Projects.Add(mProject6);
            Projects.Add(mProject);
            Projects.Add(mProject2);
            Projects.Add(mProject3);
            Projects.Add(mProject4);
            Projects.Add(mProject5);
            Projects.Add(mProject6);
            */
        }

    }
}
