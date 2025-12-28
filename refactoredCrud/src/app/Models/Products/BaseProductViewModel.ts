export abstract class BaseProductViewModel{
    productName: string;
    unitPrice: number;
    categoryId: number|null;
    constructor(productName:string,unitPrice:number,categoryId:number){
        this.productName = productName;
        this.unitPrice = unitPrice;
        this.categoryId = categoryId;
    }
}