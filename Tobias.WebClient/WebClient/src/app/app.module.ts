import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

//Environment
import { environment } from 'src/environments/environment';

//Application components
import { DonorlistComponent } from './components/donorlist/donorlist.component';
import { DonorComponent } from './components/donor/donor.component';
import { RecipientlistComponent } from './components/recipientlist/recipientlist.component';
import { RecipientComponent } from './components/recipient/recipient.component';
import { CompatibilityComponent } from './components/compatibility/compatibility.component'; 
import { HelpComponent } from './components/help/help.component';
import { AboutComponent } from './components/about/about.component';
import { AppConfig as AppConfigModule } from './config/appconfig.module';
import { ConfigurationComponent } from './components/configuration/configuration.component';

@NgModule({
  declarations: [
    AppComponent,
    DonorComponent,
    DonorlistComponent,
    RecipientComponent,
    RecipientlistComponent,
    CompatibilityComponent,
    AboutComponent,
    HelpComponent,
    ConfigurationComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    AppConfigModule,
    FormsModule, 
  ],
  providers: [
    //    { provide: "BASE_API_URL", useValue: environment.apiUrl }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
