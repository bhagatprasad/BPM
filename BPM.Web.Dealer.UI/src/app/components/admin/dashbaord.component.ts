import { Component, OnInit, AfterViewInit, ElementRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './dashbaord.component.html',
  styleUrl: './dashbaord.component.css',
})
export class DashBoardComponent implements OnInit, AfterViewInit {
  @ViewChild('totalSalesChart') totalSalesChart!: ElementRef;
  @ViewChild('profitChart') profitChart!: ElementRef;
  @ViewChild('averageDailySalesChart') averageDailySalesChart!: ElementRef;
  @ViewChild('orderSummaryChart') orderSummaryChart!: ElementRef;
  @ViewChild('revenueChart') revenueChart!: ElementRef;
  @ViewChild('salesLocationsMap') salesLocationsMap!: ElementRef;

  // Dashboard data
  stats = {
    totalOrders: 20705,
    totalCustomers: 84127,
    totalRevenue: 15278,
    totalSales: 9586,
    monthlySales: 3507,
    todaySales: 357,
    profit: 359000,
    averageDailySales: 5000,
    newCustomers: 2537,
    bestSeller: 'Michael Marquez',
    bestSellerSales: 3500
  };

  // Recent orders data
  recentOrders = [
    { id: '#ARP-1217', customer: 'Carlos Daley', avatar: 'assets/images/user1.jpg', date: '15 Nov, 2025', total: 9095, profit: 1254, status: 'Shipped' },
    { id: '#ARP-9513', customer: 'Dorothy Young', avatar: 'assets/images/user2.jpg', date: '14 Nov, 2025', total: 8564, profit: 973, status: 'Confirmed' },
    { id: '#ARP-7513', customer: 'Greg Woody', avatar: 'assets/images/user3.jpg', date: '13 Nov, 2025', total: 7985, profit: 852, status: 'Pending' },
    { id: '#ARP-3579', customer: 'Deborah Rosol', avatar: 'assets/images/user4.jpg', date: '12 Nov, 2025', total: 7362, profit: 793, status: 'Rejected' },
    { id: '#ARP-4826', customer: 'Kendall Allen', avatar: 'assets/images/user5.jpg', date: '11 Nov, 2025', total: 6597, profit: 674, status: 'Shipped' }
  ];

  // Top selling products
  topProducts = [
    { name: 'Smart Watch', image: 'assets/images/product1.png', itemsSold: 953, itemCode: '#ARP-1217', revenue: 90954 },
    { name: 'Mobile Phone', image: 'assets/images/product2.png', itemsSold: 876, itemCode: '#ARP-9513', revenue: 85648 },
    { name: 'Laptop Device', image: 'assets/images/product3.png', itemsSold: 823, itemCode: '#ARP-7531', revenue: 79852 },
    { name: 'Black T-Shirt', image: 'assets/images/product4.png', itemsSold: 743, itemCode: '#ARP-3579', revenue: 73624 },
    { name: 'Headphones', image: 'assets/images/product5.png', itemsSold: 693, itemCode: '#ARP-4826', revenue: 65973 },
    { name: 'Hand Watch', image: 'assets/images/product6.png', itemsSold: 654, itemCode: '#ARP-1265', revenue: 42455 }
  ];

  // Top sellers
  topSellers = [
    { name: 'Mark Stjohn', avatar: 'assets/images/user6.jpg', customerId: '#76431', rating: 5 },
    { name: 'Joan Stanley', avatar: 'assets/images/user7.jpg', customerId: '#64815', rating: 4.5 },
    { name: 'Jacob Bell', avatar: 'assets/images/user8.jpg', customerId: '#34581', rating: 4 },
    { name: 'Donald Bryan', avatar: 'assets/images/user9.jpg', customerId: '#67941', rating: 5 },
    { name: 'Kristina Blomquist', avatar: 'assets/images/user10.jpg', customerId: '#36985', rating: 5 },
    { name: 'Jeffrey Morrison', avatar: 'assets/images/user11.jpg', customerId: '#26985', rating: 3.5 }
  ];

  // Transactions
  transactions = [
    { title: 'Refund Bill payment', icon: 'settings_backup_restore', iconClass: 'text-primary', bgClass: 'bg-primary-10', date: '15 Nov 2025 - 11:40am', amount: 995, type: 'credit' },
    { title: 'Bank Transfer', icon: 'account_balance', iconClass: 'text-danger', bgClass: 'bg-danger-10', date: '15 Nov 2025 - 8:20am', amount: 1550, type: 'debit' },
    { title: 'Master Card', icon: 'credit_card', iconClass: 'text-primary-50', bgClass: 'bg-primary-50-10', date: '14 Nov 2025 - 11:40am', amount: 862, type: 'credit' },
    { title: 'Wallet', icon: 'account_balance_wallet', iconClass: 'text-info', bgClass: 'bg-info-10', date: '10 Nov 2025 - 10:10am', amount: 974, type: 'credit' },
    { title: 'Cash Withdrawal', icon: 'attach_money', iconClass: 'text-warning', bgClass: 'bg-warning-10', date: '09 Nov 2025 - 1:30pm', amount: 250, type: 'debit' },
    { title: 'Payment', icon: 'payments', iconClass: 'text-success', bgClass: 'bg-success-10', date: '8 Nov 2025 - 12:34pm', amount: 657, type: 'debit' }
  ];

  // New customers avatars
  newCustomers = [
    'assets/images/user12.jpg',
    'assets/images/user13.jpg',
    'assets/images/user14.jpg',
    'assets/images/user15.jpg',
    'assets/images/user16.jpg'
  ];

  // Sales locations
  locations = [
    { name: 'United States', flag: 'assets/images/usa.png', percentage: 85 },
    { name: 'China', flag: 'assets/images/china.png', percentage: 60 },
    { name: 'Australia', flag: 'assets/images/australia.png', percentage: 85 },
    { name: 'Germany', flag: 'assets/images/germany.png', percentage: 75 },
    { name: 'Canada', flag: 'assets/images/canada.png', percentage: 80 },
    { name: 'France', flag: 'assets/images/france.png', percentage: 65 }
  ];

  currentYear = '2025';
  currentPeriod = 'This Week';

  ngOnInit(): void {
    // Any initialization logic
  }

  ngAfterViewInit(): void {
    // Initialize charts if needed
    this.initCharts();
  }

  initCharts(): void {
    // Chart initialization logic would go here
    // You can use Chart.js, ECharts, or any other library
  }

  getStatusClass(status: string): string {
    const statusMap: { [key: string]: string } = {
      'Shipped': 'text-primary bg-primary bg-opacity-10',
      'Confirmed': 'text-success bg-success bg-opacity-10',
      'Pending': 'text-warning bg-warning bg-opacity-10',
      'Rejected': 'text-danger bg-danger bg-opacity-10'
    };
    return statusMap[status] || 'text-secondary bg-secondary bg-opacity-10';
  }

  getTransactionIconClass(type: string): string {
    return type === 'credit' ? 'text-success' : 'text-danger';
  }

  getStars(rating: number): any[] {
    const stars = [];
    const fullStars = Math.floor(rating);
    const hasHalfStar = rating % 1 >= 0.5;

    for (let i = 0; i < fullStars; i++) {
      stars.push({ type: 'full' });
    }
    if (hasHalfStar) {
      stars.push({ type: 'half' });
    }
    const remaining = 5 - stars.length;
    for (let i = 0; i < remaining; i++) {
      stars.push({ type: 'empty' });
    }
    return stars;
  }

  onPageChange(page: number): void {
    console.log('Page changed to:', page);
  }

  onSearch(query: string): void {
    console.log('Searching for:', query);
  }

  onFilterChange(filter: string): void {
    console.log('Filter changed to:', filter);
  }

  onPeriodChange(period: string): void {
    console.log('Period changed to:', period);
  }

  onYearChange(year: string): void {
    console.log('Year changed to:', year);
  }
}