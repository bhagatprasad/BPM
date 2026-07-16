import { Component, OnInit } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { DrugService } from '../../Services/drugs';
import { Drugs } from '../../Models/drugs';


@Component({
  selector: 'app-drug',
  standalone: true,
  imports: [CommonModule, DatePipe],
  templateUrl: './drug.html',
  styleUrl: './drug.css',
})
export class DrugComponent implements OnInit {

  drugs: Drugs[] = [];
  
  constructor(private drugService: DrugService) { }

  ngOnInit(): void {
    setTimeout(() => {
      this.getDrugs();
    }, 1000);
  }

  getDrugs(): void {
    this.drugService.getAll().subscribe(
      {
        next: (response) => {
          console.log('Raw response:', response);
          console.log('Response type:', typeof response);
          console.log('Is array?', Array.isArray(response));
          console.log('Response length:', response?.length);
          console.log('Drugs fetched successfully:', response);
          this.drugs = response;
          console.log('After assignment, drugs:', this.drugs);
          console.log('Drugs length:', this.drugs.length);
        },
        error: (error) => {
          console.error('Error fetching drugs:', error);
        }
      });

  }

}