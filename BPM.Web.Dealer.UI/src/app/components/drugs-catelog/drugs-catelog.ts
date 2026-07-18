import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { DrugCatalogService } from '../../services/drugcatelog.service';
import { drugCatelog } from '../../models/drug-catelog';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-drugs-catelog',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './drugs-catelog.html',
  styleUrl: './drugs-catelog.css',
})
export class DrugsCatelogComponent implements OnInit {
  drugsCatalogs: drugCatelog[] = [];
  filteredDrugs: drugCatelog[] = [];
  viewMode: 'grid' | 'list' = 'grid';
  searchTerm: string = '';
  isLoading: boolean = false;
  error: string | null = null;

  constructor(
    private drugCatalogService: DrugCatalogService,
    private cartService: CartService,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.fetchDrugsCatalog();
  }

  fetchDrugsCatalog(): void {
    this.isLoading = true;
    this.error = null;
    console.log('Fetching drugs catalog...');

    this.drugCatalogService.getDrugsCatalogAsync().subscribe({
      next: (response: drugCatelog[]) => {
        console.log('Drugs fetched successfully:', response);
        console.log('Number of drugs:', response?.length);

        this.drugsCatalogs = response || [];
        this.filteredDrugs = [...this.drugsCatalogs];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error fetching drugs:', error);
        this.error = 'Failed to load drugs. Please try again.';
        this.drugsCatalogs = [];
        this.filteredDrugs = [];
        this.isLoading = false;
        this.cdr.detectChanges();
      },
    });
  }

  filterDrugs(): void {
    if (!this.searchTerm.trim()) {
      this.filteredDrugs = [...this.drugsCatalogs];
      return;
    }

    const search = this.searchTerm.toLowerCase().trim();
    this.filteredDrugs = this.drugsCatalogs.filter(
      (drug) =>
        drug.drugName?.toLowerCase().includes(search) ||
        drug.drugCode?.toLowerCase().includes(search) ||
        drug.genericName?.toLowerCase().includes(search) ||
        drug.brandName?.toLowerCase().includes(search) ||
        drug.manufacturer?.toLowerCase().includes(search) ||
        drug.category?.toLowerCase().includes(search),
    );
  }

  sortDrugs(event: any): void {
    const sortBy = event.target.value;

    switch (sortBy) {
      case 'name':
        this.filteredDrugs.sort((a, b) => (a.drugName || '').localeCompare(b.drugName || ''));
        break;
      case 'code':
        this.filteredDrugs.sort((a, b) => (a.drugCode || '').localeCompare(b.drugCode || ''));
        break;
      case 'category':
        this.filteredDrugs.sort((a, b) => (a.category || '').localeCompare(b.category || ''));
        break;
      case 'status':
        this.filteredDrugs.sort((a, b) => Number(b.isActive) - Number(a.isActive));
        break;
      default:
        break;
    }
  }

  toggleView(): void {
    this.viewMode = this.viewMode === 'grid' ? 'list' : 'grid';
  }

  addToCart(drug: drugCatelog): void {
    if (!drug.isActive) {
      alert('This drug is currently inactive and cannot be added to cart.');
      return;
    }
    this.cartService.addToCart({
      drugId: drug.drugId,
      drugCode: drug.drugCode,
      drugName: drug.drugName,
      genericName: drug.genericName,
      manufacturer: drug.manufacturer,
      brandName: drug.brandName,
      category: drug.category,
      packing: drug.packing,
      strength: drug.strength,
      imageUrl: drug.imageUrl,
      quantity: 1,
    });

    console.log(this.cartService.getCartItems());
    console.log('Cart Count:', this.cartService.getCartCount());
    console.log(`${drug.drugName} will be added to cart Successfully.`);
  }

  viewDrug(drug: drugCatelog): void {
    console.log('View drug:', drug);
    alert(`Viewing details for: ${drug.drugName}`);
  }

  editDrug(drug: drugCatelog): void {
    console.log('Edit drug:', drug);
    alert(`Editing drug: ${drug.drugName}`);
  }

  deleteDrug(drug: drugCatelog): void {
    if (confirm(`Are you sure you want to delete ${drug.drugName}?`)) {
      console.log('Delete drug:', drug);
      this.drugsCatalogs = this.drugsCatalogs.filter((d) => d.drugId !== drug.drugId);
      this.filterDrugs();
      alert(`${drug.drugName} has been deleted.`);
    }
  }

  addNewDrug(): void {
    console.log('Add new drug');
    alert('Opening add drug form...');
  }
}
