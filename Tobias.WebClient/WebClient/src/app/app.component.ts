import { Component, NgModule, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppConfig } from './config/appconfig.module';
import { ItemStatesGUI } from './shared/ItemStates';
import { PageActions } from './shared/PageActions';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  //Aliases for enums, required for html to be able to access the enum values
  public PageActions = PageActions;

  //Properties
  public PageTitle = 'Tobias';
  public ServerRootUrl = '';

  constructor(private router: Router, private appConfig: AppConfig) {
    this.ServerRootUrl = appConfig.ServerRootUrl; 
  }

  public PerformAction(action: PageActions): void {
    
    console.log("Performing page action: " + PageActions[action]);

    switch (action) {
      case PageActions.ListDonors:
        this.router.navigate(['donorlist-component']);
        break;
      case PageActions.AddDonor:
        this.router.navigate(['donor-component'], { queryParams: { "guid": "", "ItemStateGUI": ItemStatesGUI[ItemStatesGUI.Editable] } });
        break;
      case PageActions.ListRecipients:
        this.router.navigate(['recipientlist-component']);
        break;
      case PageActions.AddRecipient:
        this.router.navigate(['recipient-component'], { queryParams: { "guid": "", "ItemStateGUI": ItemStatesGUI[ItemStatesGUI.Editable] } });
        break;
      case PageActions.CalculateBloodCompatibility:
        this.router.navigate(['compatibility-component']);
        break;
      case PageActions.About:
        this.router.navigate(['about-component']);
        break;
      case PageActions.Help:
        this.router.navigate(['help-component']);
        break;
      case PageActions.Configuration:
        this.router.navigate(['configuration-component']);
        break;
      }
  }
}
