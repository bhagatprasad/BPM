import { Component,OnInit} from '@angular/core';
import { CommonModule,DatePipe } from '@angular/common'; 
import { DrugService } from '../../Services/drugs';
import { Drugs } from '../../Models/drugs';

@Component({
  selector: 'app-drug',
  standalone: true,
  imports: [CommonModule,DatePipe],
  templateUrl: './drug.html',
  styleUrl: './drug.css',
})
export class DrugComponent implements OnInit  {

drugs:Drugs[] = [];
constructor(private drugService: DrugService) { }

  ngOnInit(): void
   {
     this.getDrugs();
  }

getDrugs(): void
 {
    this.drugService.getAll().subscribe(
    {
         next:(response:Drugs[]) => { this.drugs = response;},
         error:(error) => { console.error('Error fetching drugs:' , error);      
    }
  });

}

}