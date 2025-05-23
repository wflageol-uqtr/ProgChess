
export type UnitTest = {
  id: number;
  code: string;
  isActive: boolean;
};

export type Exercice = {
  id: number;
  situation: string;
  code: string;
  unitTests: UnitTest[];
  studentCodes: string[]
};
