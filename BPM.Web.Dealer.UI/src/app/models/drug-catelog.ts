export interface drugCatelog {
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
    imageUrl?: string;
}