import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { DrugCatalogService } from '../../services/drugcatelog.service';
import { drugCatelog, DrugPackaging } from '../../models/drug-catelog';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CartService } from '../../services/cart.service';
import { SpinnerLoadingService } from '../../common/services/spinner-loading-service';

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
  selectedPackages: { [drugId: string]: DrugPackaging } = {};
  selectedCategory: string = '';

  constructor(
    private drugCatalogService: DrugCatalogService,
    private cartService: CartService,
    private cdr: ChangeDetectorRef,
    private spinnerService: SpinnerLoadingService
  ) { }

  ngOnInit(): void {
    this.fetchDrugsCatalog();
  }

  fetchDrugsCatalog(): void {
    this.isLoading = true;
    this.error = null;
    this.spinnerService.show();

    this.drugCatalogService.getDrugsCatalogAsync().subscribe({
      next: (response: drugCatelog[]) => {
        console.log('Drugs fetched successfully:', response);
        this.drugsCatalogs = response || [];
        this.filteredDrugs = [...this.drugsCatalogs];
        this.isLoading = false;
        this.spinnerService.hide();
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error fetching drugs:', error);
        this.error = 'Failed to load drugs. Please try again.';
        this.drugsCatalogs = [];
        this.filteredDrugs = [];
        this.isLoading = false;
        this.spinnerService.hide();
        this.cdr.detectChanges();
      },
    });
  }

  // ========== Filter Methods ==========
  filterDrugs(): void {
    let filtered = [...this.drugsCatalogs];

    // Search filter
    if (this.searchTerm.trim()) {
      const search = this.searchTerm.toLowerCase().trim();
      filtered = filtered.filter(
        (drug) =>
          drug.drugName?.toLowerCase().includes(search) ||
          drug.drugCode?.toLowerCase().includes(search) ||
          drug.genericName?.toLowerCase().includes(search) ||
          drug.brandName?.toLowerCase().includes(search) ||
          drug.manufacturer?.toLowerCase().includes(search) ||
          drug.category?.toLowerCase().includes(search)
      );
    }

    // Category filter
    if (this.selectedCategory) {
      filtered = filtered.filter(
        (drug) => drug.category === this.selectedCategory
      );
    }

    this.filteredDrugs = filtered;
  }

  filterByCategory(event: any): void {
    this.selectedCategory = event.target.value;
    this.filterDrugs();
  }

  sortDrugs(event: any): void {
    const sortBy = event.target.value;
    const sorted = [...this.filteredDrugs];

    switch (sortBy) {
      case 'name':
        sorted.sort((a, b) => (a.drugName || '').localeCompare(b.drugName || ''));
        break;
      case 'code':
        sorted.sort((a, b) => (a.drugCode || '').localeCompare(b.drugCode || ''));
        break;
      case 'category':
        sorted.sort((a, b) => (a.category || '').localeCompare(b.category || ''));
        break;
      case 'status':
        sorted.sort((a, b) => Number(b.isActive) - Number(a.isActive));
        break;
      default:
        break;
    }

    this.filteredDrugs = sorted;
  }

  // ========== Stats Methods ==========
  getActiveDrugs(): number {
    return this.drugsCatalogs.filter(d => d.isActive).length;
  }

  getInactiveDrugs(): number {
    return this.drugsCatalogs.filter(d => !d.isActive).length;
  }

  getUniqueCategories(): number {
    const categories = new Set(this.drugsCatalogs.map(d => d.category).filter(Boolean));
    return categories.size;
  }

  getUniqueManufacturers(): number {
    const manufacturers = new Set(this.drugsCatalogs.map(d => d.manufacturer).filter(Boolean));
    return manufacturers.size;
  }

  getUniqueSchedules(): number {
    const schedules = new Set(this.drugsCatalogs.map(d => d.scheduleType).filter(Boolean));
    return schedules.size;
  }

  getCategoryList(): string[] {
    const categories = new Set(
      this.drugsCatalogs
        .map(d => d.category)
        .filter((category): category is string => category !== undefined && category !== null && category !== '')
    );
    return Array.from(categories);
  }
  // ========== UI Methods ==========
  toggleView(): void {
    this.viewMode = this.viewMode === 'grid' ? 'list' : 'grid';
  }

  selectPackage(drugId: string, pkg: DrugPackaging): void {
    if (this.selectedPackages[drugId]?.packagingId === pkg.packagingId) {
      delete this.selectedPackages[drugId];
    } else {
      this.selectedPackages[drugId] = pkg;
    }
  }

  addToCart(drug: drugCatelog): void {
    if (!drug.isActive) {
      alert('This drug is currently inactive and cannot be added to cart.');
      return;
    }

    const selectedPackage = this.selectedPackages[drug.drugId];
    if (!selectedPackage) {
      alert('Please select a package.');
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
      packageName: selectedPackage.packageUomName,
      packagePrice: selectedPackage.packagePrice,
      packagingId: selectedPackage.packagingId,
      displayName: selectedPackage.displayName,
      imageUrl: drug.imageUrl,
      quantity: 1,
    });

    console.log(this.cartService.getCartItems());
    console.log('Cart Count:', this.cartService.getCartCount());
    alert(`${drug.drugName} added to cart successfully!`);
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