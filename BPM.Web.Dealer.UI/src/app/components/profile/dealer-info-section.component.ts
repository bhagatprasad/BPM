import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-dealer-info-section',
  imports: [CommonModule, FormsModule],
  templateUrl: './dealer-info-section.component.html',
  styleUrl: './dealer-info-section.component.css',
})
export class DealerInfoSectionComponent {
  @Input() isAdmin: boolean = false;
@Input() dealerSection: any = {};
@Input() isDealerEditing: boolean = false;
@Input() errorMessage: string = '';

@Output() Edit = new EventEmitter<void>();
@Output() save = new EventEmitter<void>();
@Output() cancel = new EventEmitter<void>();
constructor(){
  console.log('DealerInfoSectionComponent intialized with isAdmin ',this.isAdmin);
  console.log('DealerInfoSectionComponent intialized with dealearsection ',this.dealerSection);
  console.log('DealerInfoSectionComponent intialized with isDealerEditing ',this.isDealerEditing);
  console.log('DealerInfoSectionComponent intialized with errorMessage ',this.errorMessage);
  console.log('DealerInfoSectionComponent intialized with Edit ',this.Edit);
  console.log('DealerInfoSectionComponent intialized with save ',this.save);
  console.log('DealerInfoSectionComponent intialized with cancel ',this.cancel);
}
ngOnChanges(changes: SimpleChanges)
{
   if(changes['isAdmin'])
    {
      console.log('isAdmin changed to',changes['isAdmin'].currentValue);
      this.isAdmin = changes['isAdmin'].currentValue;
    }
  if(changes['dealerSection'])
    {
      console.log('dealerSecrtion changed to',changes['dealerSection'].currentValue);
      this.dealerSection = changes['dealerSection'].currentValue;
    }
    if(changes['isDealerEditing'])
    {
      console.log('isDealerEditing changed to',changes['isDealerEditing'].currentValue);
      this.isDealerEditing = changes['isDealerEditing'].currentValue;
    }
    if(changes['errorMessage'])
    {
      console.log('errorMessage changed to',changes['errorMessage'].currentValue);
      this.errorMessage = changes['errorMessage'].currentValue;
    }
    if(changes['Edit'])
    {
      console.log('Edit changed to',changes['Edit'].currentValue);
      this.Edit = changes['Edit'].currentValue;
    }
    if(changes['save'])
    {
      console.log('save changed to',changes['save'].currentValue);
      this.save = changes['save'].currentValue;
    }
    if(changes['cancel'])
    {
      console.log('cancel changed to',changes['cancel'].currentValue);
      this.cancel = changes['cancel'].currentValue;
    }
}

}
