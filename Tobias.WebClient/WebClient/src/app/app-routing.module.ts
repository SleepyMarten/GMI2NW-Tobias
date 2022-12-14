import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { DonorComponent } from './components/donor/donor.component';
import { DonorlistComponent } from './components/donorlist/donorlist.component';
import { RecipientComponent } from './components/recipient/recipient.component';
import { RecipientlistComponent } from './components/recipientlist/recipientlist.component';
import { CompatibilityComponent } from './components/compatibility/compatibility.component';
import { AboutComponent } from './components/about/about.component';
import { HelpComponent } from './components/help/help.component';
import { ConfigurationComponent } from './components/configuration/configuration.component';


const routes: Routes = [
    {path:'donor-component', component: DonorComponent},
    {path:'donorlist-component', component: DonorlistComponent},
    {path:'recipient-component', component: RecipientComponent},
    {path:'recipientlist-component', component: RecipientlistComponent},
    {path:'compatibility-component', component: CompatibilityComponent},
    {path:'about-component', component: AboutComponent},
    {path:'help-component', component: HelpComponent},
    {path:'configuration-component', component: ConfigurationComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
