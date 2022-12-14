import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

/* Application-specific types */
import { Donor } from '../../shared/donor';
import { DonorService } from '../../services/donor.service';
import { ItemStatesGUI, ItemStatesDb, PageStates } from 'src/app/shared/ItemStates';


@Component({
  selector: 'app-donorlist',
  templateUrl: './donorlist.component.html',
  styleUrls: ['./donorlist.component.css'],
})
export class DonorlistComponent implements OnInit {

  //Aliases for enums, required for html to be able to access the enum values
  public PageStates = PageStates;
  public ItemStatesGUI = ItemStatesGUI;
  public ItemStatesDb = ItemStatesDb;

  //Properties
  public PageState: PageStates = PageStates.Uninitialized;
  public ItemStateGUI: ItemStatesGUI = ItemStatesGUI.ReadOnly;
  public ItemStateDb: ItemStatesDb = ItemStatesDb.Unsaved;


  //Item of this page
  public donors: Donor[] = [];

  constructor(public donorService: DonorService, private router: Router) {

  }

  private FetchDonors(): void {

    console.log("DonorlistComponent.FetchDonors()");

    this.PageState = PageStates.Fetching;
    this.donors = [];

    this.donorService.FetchDonors().subscribe(donors => {
      this.donors = donors;
      this.PageState = PageStates.FetchedOrNew;
    });
  }

  private RouteToDonorComponent(guid: string, itemStateGUI: ItemStatesGUI) : void {

    this.PageState = PageStates.Leaving;
    this.router.navigate(['donor-component'], { queryParams: { "guid": guid, "ItemStateGUI": ItemStatesGUI[itemStateGUI] } });
  }

  public ActionOpen(guid: string): void {
    console.log("DonorlistComponent.ActionOpen(guid: " + guid + ")");
    this.RouteToDonorComponent(guid, ItemStatesGUI.ReadOnly);
  }

  public ActionEdit(guid: string): void {
    console.log("DonorlistComponent.ActionEdit(guid: " + guid + ")");
    this.RouteToDonorComponent(guid, ItemStatesGUI.Editable);
  }

  public ActionDelete(guid: string): void {
    console.log("Delete clicked, with guid " + guid);
    this.PageState = PageStates.Deleting;

    this.donorService.DeleteDonor(guid).subscribe(_ => {
      this.FetchDonors();
    });
  }

  public ActionAdd(): void {
    console.log("Add clicked");

    this.PageState = PageStates.Leaving;

    this.router.navigate(['donor-component'], { queryParams: { "guid": "", "ItemStateGUI": ItemStatesGUI[ItemStatesGUI.Editable] } });
  }

  ngOnInit(): void {
    console.log("DonorlistComponent.ngOnInit()")

    this.FetchDonors();
  }

}
