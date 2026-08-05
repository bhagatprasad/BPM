import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output, SimpleChanges } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-personal-info',
  imports: [CommonModule, FormsModule],
  templateUrl: './user-personal-info.component.html',
  styleUrl: './user-personal-info.component.css',
})
export class UserPersonalInfoComponent {
  @Input() userId: string = '';
  @Input() userPersionalSection: any = {};
  @Input() isUserEditing: boolean = false;
  @Input() errorMsz: string = '';

  @Output() Edit = new EventEmitter<void>();
  @Output() save = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  constructor() {
    console.log('UserPersonalInfoComponent initialized');
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['userId']) {
      console.log('User ID changed to:', changes['userId'].currentValue);
      this.userId = changes['userId'].currentValue;
    }
    if (changes['userPersionalSection']) {
      console.log('User Data changed to:', changes['userPersionalSection'].currentValue);
      this.userPersionalSection = changes['userPersionalSection'].currentValue;
    }
    if (changes['isUserEditing']) {
      console.log('isUserEditing changed to:', changes['isUserEditing'].currentValue);
      this.isUserEditing = changes['isUserEditing'].currentValue;
    }
    if (changes['errorMsz']) {
      console.log('errorMsz changed to:', changes['errorMsz'].currentValue);
      this.errorMsz = changes['errorMsz'].currentValue;
    }
  }
  OnSave(): void {
    this.save.emit();
  }
  OnCancel(): void {
    this.cancel.emit();
  }
}