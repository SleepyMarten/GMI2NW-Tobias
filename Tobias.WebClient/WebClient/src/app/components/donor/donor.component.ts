import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

/* Application-specific types */
import { Donor } from '../../shared/donor';
import { DonorService } from '../../services/donor.service';
import { ItemStatesGUI, ItemStatesDb, PageStates } from 'src/app/shared/ItemStates';


@Component({
  selector: 'app-donor',
  templateUrl: './donor.component.html',
  styleUrls: ['./donor.component.css']
})
export class DonorComponent implements OnInit {

  //Aliases for enums, required for html to be able to access the enum values
  public PageStates = PageStates;
  public ItemStatesGUI = ItemStatesGUI;
  public ItemStatesDb = ItemStatesDb;

  //Properties
  public PageState: PageStates = PageStates.Uninitialized;
  public ItemStateGUI: ItemStatesGUI = ItemStatesGUI.ReadOnly;
  public ItemStateDb: ItemStatesDb = ItemStatesDb.Unsaved;

  //Item of this page
  public donor: Donor = new Donor;

  constructor(public donorService: DonorService, private router: Router, private activatedRoute: ActivatedRoute) { 
    console.log("donor.component.constructor, donor = " + JSON.stringify(this.donor));        
  }

  private FetchDonor(guid : string): void {
    console.log("Fetching donor with guid = " + guid);

    this.PageState = PageStates.Fetching;

    this.donorService.FetchDonor(guid).subscribe(donor => {
      this.donor = donor;
      console.log("Donor fetched with name: " + this.donor.firstName + " " + this.donor.lastName);
      this.PageState = PageStates.FetchedOrNew;
      this.ItemStateDb = ItemStatesDb.Stored;
    });
  }

  private CreateNewDonor() : void { 
    this.donor = new Donor;
    this.donor.guid = "";
    this.donor.firstName="Please enter first name";
    this.donor.lastName="Please enter last name";
    console.log("Donor created with name: " + this.donor.firstName + " " + this.donor.lastName);
    this.PageState = PageStates.FetchedOrNew;
    this.ItemStateDb = ItemStatesDb.Unsaved;
}

    public ActionEdit(): void {
      console.log("DonorComponent.ActionEdit()");
      this.ItemStateGUI = ItemStatesGUI.Editable;
    }
  
    public ActionSave(): void {
      console.log("DonorComponent.ActionSave()");

      this.PageState = PageStates.Saving; 
      this.ItemStateGUI = ItemStatesGUI.ReadOnly; 
      this.donorService.PostDonor(this.donor).subscribe(donor => {
        console.log("Donor Post returned: " + JSON.stringify(donor));
        this.donor = donor;
        this.FetchDonor(this.donor.guid);
      });
    }
  
    public ActionDelete(): void {
      console.log("DonorComponent.ActionDelete()")
      this.PageState = PageStates.Deleting ;
      this.ItemStateGUI = ItemStatesGUI.ReadOnly ;
  
      this.donorService.DeleteDonor(this.donor.guid).subscribe(_ => {
        this.ItemStateGUI = ItemStatesGUI.ReadOnly ;
        this.PageState = PageStates.Leaving;
        this.router.navigate(['donorlist-component']);
      });
    }
  
  ngOnInit(): void {

    console.log("DonorComponent:  ngOnInit()")

    this.activatedRoute.queryParamMap.subscribe(params => {
      this.ItemStateGUI = ItemStatesGUI.ReadOnly;

      var guidStr = params.get("guid");
      var guid: string = guidStr == null ? "": guidStr; 
      var itemStateGUIParamStr: any = params.get("ItemStateGUI");
      var itemStateGUIParam: ItemStatesGUI = (<any>ItemStatesGUI)[itemStateGUIParamStr];
      if(guid == "")
      {
        console.log("Creating new Donor...");
        this.CreateNewDonor();
        this.ItemStateDb = ItemStatesDb.Unsaved;
        this.ItemStateGUI = itemStateGUIParam;
      }
      else
      {
        console.log("Fetching Donor...");
        this.FetchDonor(guid);
        this.ItemStateGUI = itemStateGUIParam;
      }
    });
  }

}
