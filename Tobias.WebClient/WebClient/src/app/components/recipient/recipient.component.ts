import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

/* Application-specific types */
import { Recipient } from '../../shared/recipient';
import { RecipientService } from '../../services/recipient.service';
import { ItemStatesGUI, ItemStatesDb, PageStates } from 'src/app/shared/ItemStates';


@Component({
  selector: 'app-recipient',
  templateUrl: './recipient.component.html',
  styleUrls: ['./recipient.component.css']
})
export class RecipientComponent implements OnInit {

  //Aliases for enums, required for html to be able to access the enum values
  public PageStates = PageStates;
  public ItemStatesGUI = ItemStatesGUI;
  public ItemStatesDb = ItemStatesDb;

  //Properties
  public PageState: PageStates = PageStates.Uninitialized;
  public ItemStateGUI: ItemStatesGUI = ItemStatesGUI.ReadOnly;
  public ItemStateDb: ItemStatesDb = ItemStatesDb.Unsaved;

  //Item of this page
  public recipient: Recipient = new Recipient;

  constructor(public recipientService: RecipientService, private router: Router, private activatedRoute: ActivatedRoute) { 
    console.log("recipient.component.constructor, recipient = " + JSON.stringify(this.recipient));        
  }

  private FetchRecipient(guid : string): void {
    console.log("Fetching recipient with guid = " + guid);

    this.PageState = PageStates.Fetching;

    this.recipientService.FetchRecipient(guid).subscribe(recipient => {
      this.recipient = recipient;
      console.log("Recipient fetched with name: " + this.recipient.firstName + " " + this.recipient.lastName);
      this.PageState = PageStates.FetchedOrNew;
      this.ItemStateDb = ItemStatesDb.Stored;
    });
  }

  private CreateNewRecipient() : void { 
    this.recipient = new Recipient;
    this.recipient.guid = "";
    this.recipient.firstName="Please enter first name";
    this.recipient.lastName="Please enter last name";
    console.log("Recipient created with name: " + this.recipient.firstName + " " + this.recipient.lastName);
    this.PageState = PageStates.FetchedOrNew;
    this.ItemStateDb = ItemStatesDb.Unsaved;
}

    public ActionEdit(): void {
      console.log("RecipientComponent.ActionEdit()");
      this.ItemStateGUI = ItemStatesGUI.Editable;
    }
  
    public ActionSave(): void {
      console.log("RecipientComponent.ActionSave()");

      this.PageState = PageStates.Saving; 
      this.ItemStateGUI = ItemStatesGUI.ReadOnly; 
      this.recipientService.PostRecipient(this.recipient).subscribe(recipient => {
        console.log("Recipient Post returned: " + JSON.stringify(recipient));
        this.recipient = recipient;
        this.FetchRecipient(this.recipient.guid);
      });
    }
  
    public ActionDelete(): void {
      console.log("RecipientComponent.ActionDelete()")
      this.PageState = PageStates.Deleting ;
      this.ItemStateGUI = ItemStatesGUI.ReadOnly ;
  
      this.recipientService.DeleteRecipient(this.recipient.guid).subscribe(_ => {
        this.ItemStateGUI = ItemStatesGUI.ReadOnly ;
        this.PageState = PageStates.Leaving;
        this.router.navigate(['recipientlist-component']);
      });
    }
  
  ngOnInit(): void {

    console.log("RecipientComponent:  ngOnInit()")

    this.activatedRoute.queryParamMap.subscribe(params => {
      this.ItemStateGUI = ItemStatesGUI.ReadOnly;

      var guidStr = params.get("guid");
      var guid: string = guidStr == null ? "": guidStr; 
      var itemStateGUIParamStr: any = params.get("ItemStateGUI");
      var itemStateGUIParam: ItemStatesGUI = (<any>ItemStatesGUI)[itemStateGUIParamStr];
      if(guid == "")
      {
        console.log("Creating new Recipient...");
        this.CreateNewRecipient();
        this.ItemStateDb = ItemStatesDb.Unsaved;
        this.ItemStateGUI = itemStateGUIParam;
      }
      else
      {
        console.log("Fetching Recipient...");
        this.FetchRecipient(guid);
        this.ItemStateGUI = itemStateGUIParam;
      }
    });
  }

}
