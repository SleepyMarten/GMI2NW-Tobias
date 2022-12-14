//States of a page displaying an item/items 
export enum PageStates
{
    Uninitialized,  //State in constructor
    Fetching,       //Item/items to display on the page is being fetched
    FetchedOrNew,   //Item/items to display on the page is fetched 
    Saving,         //Currently saving an item, should be followed soon by Loading -> Loaded 
    Deleting,       //Currently deleting an item, should be followed soon by a new item Loading -> Loaded, or Leaving page 
    Leaving         //About to leave the page, should basically never be visible
}

//Used to control the state of text boxes etc.
export enum ItemStatesGUI
{
    ReadOnly,
    Editable
}

//Used to control appearance of controls depending on whether an item is stored in the database
export enum ItemStatesDb
{
    Unsaved,    //Not stored in database, does NOT have a valid guid
    Stored,     //Stored in database, has a valid guid
    Deleted     //Should basically only be used a very short period of time while leaving the page, or re-fetching items 
}