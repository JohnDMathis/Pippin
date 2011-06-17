using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MI.Authentication.ViewModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Commands;
using Pippin.UI.ViewModel;
using Pippin.UI.Events;
using Pippin.Testing;
using System.Collections.Generic;
using System.ServiceModel.DomainServices.Client;
using MI.Services;
using MI.Model;

namespace MI.Authentication.Specs
{
    // acceptance criteria
    // 1. The user cannot attempt login without providing a username and password
    // 2. A client-generated validation key must also be provided, and it must match the expected key
    // 3. If the user is not authenticated, the user must be informed of the failure
    // 4. If the user is authenticated, the currentUser must be populated, and a login event raised
    // 5. User must specify whether or not to remember the login
    // 6. If this is a returning user with a remembered login, the login window must be skipped, and the login event raised immediately
    // 7. Set state to Ready initially.
    // 8. Set state to Busy while waiting for service
    // 9. Set state to Authenticated when authenticated
    // 10. Set state to InvalidCredential when user/password failed
    // 11. Set state to Ready when user acknowledges error.

    public class EntryScreenSpec : WorkItemTest
    {
        public const string Tag = "EntryScreen";
        protected EntryViewModel _viewModel;
        protected IUnityContainer _container = new UnityContainer();
        protected MockAuthenticationClient _client;
        protected AppUser _testUser { get; set; }
        public static AppUser LogonEventUser;
        public static bool CheckLoginTestResult;

        protected virtual void Setup()
        {
            Setup(null);
        }

        protected virtual void Setup(List<Entity> entities)
        {
            List<Entity> _entities = new List<Entity>();
            if (entities != null)
                _entities = entities;
            else
            {
                //_entities.Add(someentity);

            }
            _client = new MockAuthenticationClient(entities);
            _container
                .RegisterType<EntryViewModel>()
               .RegisterType<IEventAggregator, MockEventAggregator>()
               .RegisterType<AuthenticationContext>(new ContainerControlledLifetimeManager(), new InjectionConstructor(_client));

            _viewModel = _container.Resolve<EntryViewModel>();
        }
    }

    #region [ The user cannot attempt login without providing a username and password ]

    [TestClass]
    public class When_the_login_view_is_first_shown:EntryScreenSpec
    {
        [TestMethod]
        public void It_should_have_the_Login_button_disabled()
        {
            Setup();
            _viewModel.CanLogin.ShouldBeFalse();
        }

        [TestMethod]
        public void It_should_have_empty_username_and_password_fields()
        {
            Setup();
            _viewModel.UserName.ShouldEqual(string.Empty);
            _viewModel.Password.ShouldEqual(string.Empty);
        }

        [TestMethod]
        public void The_remember_me_checkbox_should_be_unchecked()
        {
            Setup();
            _viewModel.RememberMe.ShouldBeFalse();
        }
    }

    [TestClass]
    public class When_username_is_empty_and_password_is_empty : EntryScreenSpec
    {
        [TestMethod]
        public void The_login_button_should_be_disabled()
        {
            Setup();
            _viewModel.UserName = "";
            _viewModel.Password = "";
            _viewModel.CanLogin.ShouldBeFalse();
        }
    }

    [TestClass]
    public class When_username_is_empty_and_password_is_not_empty : EntryScreenSpec
    {
        [TestMethod]
        public void The_login_button_should_be_disabled()
        {
            Setup();
            _viewModel.UserName = "";
            _viewModel.Password = "test";
            _viewModel.CanLogin.ShouldBeFalse();
        }
    }

    [TestClass]
    public class When_username_is_not_empty_and_password_is_empty : EntryScreenSpec
    {
        [TestMethod]
        public void The_login_button_should_be_disabled()
        {
            Setup();
            _viewModel.UserName = "test";
            _viewModel.Password = "";
            _viewModel.CanLogin.ShouldBeFalse();
        }
    }

    [TestClass]
    public class When_username_is_not_empty_and_password_is_not_empty : EntryScreenSpec
    {
        [TestMethod]
        public void The_login_button_should_be_enabled()
        {
            Setup();
            _viewModel.UserName = "test";
            _viewModel.Password = "test";
            _viewModel.CanLogin.ShouldBeTrue();
        }

    }

    [TestClass]
    public class When_user_attempts_default_command_and_username_is_empty : EntryScreenSpec
    {

        [TestMethod]
        public void it_should_not_attempt_to_authenticate()
        {
            Setup();
            _viewModel.UserName = "";
            _viewModel.Password = "test";
            _viewModel.RequestLogin();
            _viewModel.State.ShouldEqual(LoginState.Ready);
        }

    }

    [TestClass]
    public class When_user_attempts_default_command_and_password_is_empty : EntryScreenSpec
    {

        [TestMethod]
        public void it_should_not_attempt_to_authenticate()
        {
            Setup();
            _viewModel.UserName = "test";
            _viewModel.Password = "";
            _viewModel.RequestLogin();
            _viewModel.State.ShouldEqual(LoginState.Ready);
        }
    }

    [TestClass]
    public class When_user_attempts_default_command : EntryScreenSpec
    {

        [TestMethod]
        public void it_should_attempt_to_authenticate()
        {
            Setup();
            _viewModel.UserName = "test";
            _viewModel.Password = "test";
            _viewModel.RequestLogin();
            _viewModel.State.ShouldEqual(LoginState.Authenticating);
        }
    }



        #endregion [ The user cannot attempt login without providing a username and password ]

    [TestClass]
    public class When_resolving_the_ViewModel : EntryScreenSpec
    {
        [TestMethod]
        public void it_should_inject_the_AuthenticationContext()
        {
            Setup();
            _viewModel.Context.ShouldBeOfType(typeof(AuthenticationContext));
        }

        [TestMethod]
        public void it_should_inject_the_EventAggregator()
        {
            Setup();
            _viewModel.EventMgr.ShouldBeOfType(typeof(MockEventAggregator));
        }

        [TestMethod]
        public void viewModel_should_have_correct_type()
        {
            Setup();
            _viewModel.ShouldBeOfType(typeof(ViewModelBase));
        }

        [TestMethod]
        public void it_should_initialize_the_LogonCommand()
        {
            Setup();
            _viewModel.LoginCommand.ShouldBeOfType(typeof(DelegateCommand));
            _viewModel.LoginCommand.ShouldNotBeNull();
        }

        [TestMethod]
        public void it_should_initialize_the_AcknowledgeErrorCommand()
        {
            Setup();
            _viewModel.AcknowledgeErrorCommand.ShouldBeOfType(typeof(DelegateCommand));
            _viewModel.AcknowledgeErrorCommand.ShouldNotBeNull();
        }
    }


    [TestClass]
    public class When_started : EntryScreenSpec
    {
        [TestMethod]
        public void the_state_should_be_CheckingAuthentication()
        {
            Setup();
            _viewModel.Start();
            _viewModel.State.ShouldEqual(LoginState.CheckingAuthentication);
        }

        [TestMethod, Asynchronous]
        public void it_should_check_to_determine_whether_the_user_is_already_authenticated()
        {
            Setup();
            _viewModel.Start();
            EnqueueDelay(25);
            EnqueueCallback(() => _client.InvokeOperations.ShouldContain("GetAuthenticatedUser"));
            EnqueueTestComplete();
        }
    }

    [TestClass]
    public class When_starting_and_user_is_already_authenticated : EntryScreenSpec
    {
        [TestMethod, Asynchronous]
        public void it_should_should_send_a_logon_event_with_the_authenticated_user()
        {
            Setup();
            _client.LoginTestUser = new AppUser { Id = 1, FirstName = "Fred", UserName = "FredT", Email = "Fred@Someco.com" };
            EntryScreenSpec.LogonEventUser = null;
            _viewModel.Start();
            EnqueueDelay(75);
            EnqueueCallback(() =>
            {
                _viewModel.State.ShouldEqual(LoginState.Authenticated);
                EntryScreenSpec.LogonEventUser.ShouldBeOfType(typeof(AppUser));
                EntryScreenSpec.LogonEventUser.Email.ShouldEqual("Fred@Someco.com");
            });
            EnqueueTestComplete();
        }
    }

    [TestClass]
    public class When_starting_and_user_is_not_already_authenticated : EntryScreenSpec
    {
        [TestMethod, Asynchronous]
        public void it_should_set_the_state_to_ready()
        {
            Setup();
            _client.LoginTestUser = null;
            EntryScreenSpec.LogonEventUser = null;
            _viewModel.Start();
            EnqueueDelay(75);
            EnqueueCallback(() =>
            {
                _viewModel.State.ShouldEqual(LoginState.Ready);
                EntryScreenSpec.LogonEventUser.ShouldBeNull();
            });
            EnqueueTestComplete();
        }
    }

    
    [TestClass]
    public class When_requesting_authentication : EntryScreenSpec
    {
        protected override void Setup()
        {
            _testUser = new AppUser { Id = 1, FirstName = "Fred", UserName = "FredT", Email = "Fred@Someco.com" };
            List<Entity> entities = new List<Entity>();
            entities.Add(_testUser);
            Setup(entities);
            _viewModel.UserName = "FredT";
            _viewModel.Password = "test";
        }

        [TestMethod, Asynchronous]
        public void it_should_provide_an_authentication_key()
        {
            Setup();
            _viewModel.RequestLogin();
            EnqueueDelay(25);
            EnqueueCallback(() => _client.AuthenticationKeyProvided.ShouldBeTrue());
            EnqueueTestComplete();
        }

        [TestMethod, Asynchronous]
        public void it_should_provide_a_username()
        {
            Setup();
            _viewModel.RequestLogin();
            EnqueueDelay(25);
            EnqueueCallback(() => _client.userNameProvided.ShouldEqual("FredT"));
            EnqueueTestComplete();
        }

        [TestMethod, Asynchronous]
        public void it_should_provide_a_password()
        {
            Setup();
            _viewModel.RequestLogin();
            EnqueueDelay(25);
            EnqueueCallback(() => _client.passwordProvided.ShouldEqual("test"));
            EnqueueTestComplete();
        }
    }

    [TestClass]
    public class While_waiting_for_the_authentication_service_to_complete : EntryScreenSpec
    {
        [TestMethod]
        public void the_state_should_be_Authenticating()
        {
            Setup();
            _viewModel.UserName = "Fred";
            _viewModel.Password = "123";
            _viewModel.RequestLogin();
            _viewModel.State.ShouldEqual(LoginState.Authenticating);
        }
    }

    [TestClass]
    public class When_requesting_authentication_with_valid_credentials : EntryScreenSpec
    {
        protected override void Setup()
        {
            //List<Entity> entities = new List<Entity>();
            //entities.Add(_testUser);
            base.Setup();
            _client.LoginTestUser = new AppUser { Id = 1, FirstName = "Fred", UserName = "FredT", Email = "Fred@Someco.com" };
            _viewModel.UserName = "FredT";
            _viewModel.Password = "test";
        }

        [TestMethod, Asynchronous]
        public void it_should_return_a_matching_user_entity()
        {
            Setup();
            _viewModel.RequestLogin();
            EnqueueDelay(25);
            EnqueueCallback(() => _viewModel.AuthenticatedUser.FirstName.ShouldEqual("Fred"));
            EnqueueTestComplete();
        }

        [TestMethod, Asynchronous]
        public void it_should_indicate_the_user_is_authenticated()
        {
            Setup();
            _viewModel.RequestLogin();
            EnqueueDelay(25);
            EnqueueCallback(() => _viewModel.State.ShouldEqual(LoginState.Authenticated));
            EnqueueTestComplete();
        }

        [TestMethod, Asynchronous]       
        public void it_should_send_the_logonEvent_with_the_user()
        {
            Setup();
            EntryScreenSpec.LogonEventUser = null;
            _viewModel.RequestLogin();
            EnqueueDelay(25);
            EnqueueCallback(() => 
            {
                EntryScreenSpec.LogonEventUser.ShouldBeOfType(typeof(AppUser));
                EntryScreenSpec.LogonEventUser.Email.ShouldEqual("Fred@Someco.com");
            });
            EnqueueTestComplete();
        }


    }
   
    [TestClass]
    public class When_requesting_authentication_with_invalid_credentials : EntryScreenSpec
    {
        protected override void Setup()
        {
            _testUser = new AppUser { Id = 1, FirstName = "Fred", UserName = "FredT", Email = "Fred@Someco.com" };
            List<Entity> entities = new List<Entity>();
            entities.Add(_testUser);
            Setup(entities);
            _viewModel.UserName = "BobS";
            _viewModel.Password = "again";
        }

        [TestMethod, Asynchronous]
        public void it_should_return_a_null_value()
        {
            Setup();
            _viewModel.RequestLogin();
            EnqueueDelay(25);
            EnqueueCallback(() => _viewModel.AuthenticatedUser.ShouldBeNull());
            EnqueueTestComplete();
        }

        [TestMethod, Asynchronous]
        public void it_should_display_an_error_and_wait_for_the_user_to_acknowledge()
        {
            Setup();
            _viewModel.RequestLogin();
            EnqueueDelay(25);
            EnqueueCallback(() => _viewModel.State.ShouldEqual(LoginState.InvalidCredential));
            EnqueueTestComplete();
        }

        [TestMethod, Asynchronous]
        public void it_should_reset_the_invalid_username_and_password()
        {
            Setup();
            _viewModel.RequestLogin();
            EnqueueDelay(25);
            EnqueueCallback(() =>
            {
                _viewModel.UserName.ShouldEqual(string.Empty);
                _viewModel.Password.ShouldEqual(string.Empty);
            });
            EnqueueTestComplete();
        }
    }

    [TestClass]
    public class When_user_acknowledges_failed_login : EntryScreenSpec
    {
        [TestMethod]
        public void it_should_change_state_to_ready()
        {
            Setup();
            _viewModel.Start();

            _viewModel.State = LoginState.InvalidCredential;
            _viewModel.LoginFailureAcknowledged();
            _viewModel.State.ShouldEqual(LoginState.Ready);
        }
    }

    [TestClass, Tag("Logout")]
    public class When_user_requests_logout:EntryScreenSpec
    {
        [TestMethod, Asynchronous]
        public void it_should_invoke_logout()
        {
            Setup();
            _viewModel.State = LoginState.Authenticated;
            MockEventAggregator.ExpectedType = typeof(LogoffEvent);
            _viewModel.RequestLogout();
            EnqueueDelay(25);
            EnqueueCallback(() => _client.InvokeOperations.ShouldContain("InvokeLogout"));
            EnqueueTestComplete();
        }

        [TestMethod]
        public void it_should_change_state_to_ready()
        {
            Setup();
            _viewModel.State = LoginState.Authenticated;
            MockEventAggregator.ExpectedType = typeof(LogoffEvent);
            _viewModel.RequestLogout();
            _viewModel.State.ShouldEqual(LoginState.Ready);
        }
    }

}