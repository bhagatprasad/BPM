export class Drugs {   

  drugId: string;
  drugCode: string;
  drugName: string;
  genericName?: string;
  brandName?: string;
  manufacturer?: string;
  category?: string;
  hsnCode?: string;
  scheduleType?: string;
  packing?: string;
  strength?: string;
  isActive: boolean;
  createdBy?: string;
  createdOn: Date;
  modifiedBy?: string;
  modifiedOn?: Date;

  constructor() {
    this.drugId = '';
    this.drugCode = '';
    this.drugName = '';
    this.isActive = true;
    this.createdOn = new Date();
  }
}

