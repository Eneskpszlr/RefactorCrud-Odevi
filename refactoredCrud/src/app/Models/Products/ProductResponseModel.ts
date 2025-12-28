import { BaseProductViewModel } from "./BaseProductViewModel";

export class ProductResponseModel extends BaseProductViewModel{
    id:number;
    constructor(productName:string,unitPrice:number,categoryId:number,id:number){
        super(productName,unitPrice,categoryId);
        this.id = id;
    }
}