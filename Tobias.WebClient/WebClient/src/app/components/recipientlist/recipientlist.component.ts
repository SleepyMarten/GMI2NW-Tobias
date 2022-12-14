import { Component, Inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';

/* Application-specific types */
import { Recipient } from '../../shared/recipient';
import { RecipientService } from '../../services/recipient.service';
import { ItemStatesGUI, ItemStatesDb, PageStates } from 'src/app/shared/ItemStates';


@Component({
  selector: 'app-recipientlist',
  templateUrl: './recipientlist.component.html',
  styleUrls: ['./recipientlist.component.css'],
})
export class RecipientlistComponent implements OnInit {

  //Aliases for enums, required for html to be able to access the enum values
  public PageStates = PageStates;
  public ItemStatesGUI = ItemStatesGUI;
  public ItemStatesDb = ItemStatesDb;

  //Properties
  public PageState: PageStates = PageStates.Uninitialized;
  public ItemStateGUI: ItemStatesGUI = ItemStatesGUI.ReadOnly;
  public ItemStateDb: ItemStatesDb = ItemStatesDb.Unsaved;


  //Item of this page
  public recipients: Recipient[] = [];

  constructor(public recipientService: RecipientService, private router: Router) {

  }

  private FetchRecipients(): void {

    console.log("RecipientlistComponent.FetchRecipients()");

    this.PageState = PageStates.Fetching;
    this.recipients = [];

    this.recipientService.FetchRecipients().subscribe(recipients => {
      this.recipients = recipients;
      this.PageState = PageStates.FetchedOrNew;
    });
  }

  private RouteToRecipientComponent(guid: string, itemStateGUI: ItemStatesGUI) : void {

    this.PageState = PageStates.Leaving;
    this.router.navigate(['recipient-component'], { queryParams: { "guid": guid, "ItemStateGUI": ItemStatesGUI[itemStateGUI] } });
  }

  public ActionOpen(guid: string): void {
    console.log("RecipientlistComponent.ActionOpen(guid: " + guid + ")");
    this.RouteToRecipientComponent(guid, ItemStatesGUI.ReadOnly);
  }

  public ActionEdit(guid: string): void {
    console.log("RecipientlistComponent.ActionEdit(guid: " + guid + ")");
    this.RouteToRecipientComponent(guid, ItemStatesGUI.Editable);
  }

  public ActionDelete(guid: string): void {
    console.log("Delete clicked, with guid " + guid);
    this.PageState = PageStates.Deleting;

    this.recipientService.DeleteRecipient(guid).subscribe(_ => {
      this.FetchRecipients();
    });
  }

  public ActionAdd(): void {
    console.log("Add clicked");

    this.PageState = PageStates.Leaving;

    this.router.navigate(['recipient-component'], { queryParams: { "guid": "", "ItemStateGUI": ItemStatesGUI[ItemStatesGUI.Editable] } });
  }

  ngOnInit(): void {
    console.log("RecipientlistComponent.ngOnInit()")

    this.FetchRecipients();
  }

}
