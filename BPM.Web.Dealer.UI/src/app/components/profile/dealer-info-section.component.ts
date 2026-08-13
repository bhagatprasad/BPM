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

  @Output() edit = new EventEmitter<void>();
  @Output() save = new EventEmitter<any>();
  @Output() cancel = new EventEmitter<void>();

  constructor() {}

  ngOnChanges(changes: SimpleChanges) {
    if (changes['isAdmin']) {
      console.log('isAdmin changed to:', changes['isAdmin'].currentValue);    
    }
    if (changes['dealerSection']) {
      console.log('dealerSection changed to:', changes['dealerSection'].currentValue);      
    }
    if (changes['isDealerEditing']) {
      console.log('isDealerEditing changed to:', changes['isDealerEditing'].currentValue);      
    if (changes['errorMessage']) {
      console.log('errorMessage changed to:', changes['errorMessage'].currentValue);     
    }
}
  }
onEdit():void{
  this.edit.emit();
}
onSave():any{
  this.save.emit(this.dealerSection);
}
onCancel():void{
  this.cancel.emit();
}
}