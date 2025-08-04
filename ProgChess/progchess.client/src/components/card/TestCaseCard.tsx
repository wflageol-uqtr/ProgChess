import { PanelRightClose, RefreshCcw } from "lucide-react";
import type { EditorError, TabType, TestResult } from "../../utils/type";
import { Tabs } from "../tabs/Tabs";
import TestResultPanel from "../panel/TestResultPanel";
import CodeEditor from "../form/input/CodeEditor";
import { Button } from "../ui/button";
import { useState } from "react";
import { DeleteDialog } from "../dialog/DeleteDialog";

interface TestCaseCardProps {
  isPending: boolean;
  testResult: TestResult[];
  unitTestCode: string;
  setTestCode: React.Dispatch<React.SetStateAction<string>>;
  reinitializeFn: () => void;
  executionError?: EditorError;
}

export default function TestCaseCard({
  isPending,
  testResult,
  unitTestCode,
  setTestCode,
  reinitializeFn,
  executionError,
}: TestCaseCardProps) {
  const [openDeleteDialog, setOpenDeleteDialog] = useState(false);

  const tabs: TabType[] = [
    {
      id: 0,
      name: "Résultat",
      isActive: true,
      component: (
        <TestResultPanel isPending={isPending} testResult={testResult} />
      ),
    },
    {
      id: 1,
      name: "Test unitaire",
      isActive: false,
      component: (
        <CodeEditor
          value={unitTestCode}
          height={400}
          onChange={(e) => setTestCode(e)}
          executionError={
            executionError?.id === "test0" ? executionError : undefined
          }
        />
      ),
    },
    {
      id: 2,
      name: "Erreur d'exécution",
      isActive: false,
      component: (
        <>
          {executionError ? (
            <>
              <h3 className="text-lg font-semibold text-red-500 mb-2">
                Erreur lors de l'exécution
              </h3>
              <p className="text-sm text-red-600 leading-relaxed">
                {executionError?.error ?? ""}
              </p>
            </>
          ) : (
            <div className="flex w-full h-64 justify-center items-center text-zinc-400">
              Exécuter les tests pour voir une sortie
            </div>
          )}
        </>
      ),
    },
  ];

  return (
    <>
      <div className="bg-zinc-800 rounded-2xl h-full min-h-[300px] flex flex-col overflow-auto">
        <div className="flex justify-between w-full p-2 items-center bg-zinc-700 rounded-t-2xl">
          <div className="flex gap-2 items-center">
            <PanelRightClose className="text-green-500" />
            <h4 className="font-semibold text-xl">Résultats des tests</h4>
          </div>
          <div>
            <Button
              className="bg-zinc-500 hover:bg-zinc-600 cursor-pointer"
              type="button"
              onClick={(e) => {
                e.stopPropagation();
                setOpenDeleteDialog(true);
              }}
            >
              <RefreshCcw />
              <div className="hidden md:flex">Réinitialiser</div>
            </Button>
          </div>
        </div>
        <Tabs tabs={tabs} />
      </div>
      <DeleteDialog
        open={openDeleteDialog}
        message="Cette action est irréversible. Les tests unitaire que vous avez jusqu'à présent sera perdu."
        onOpenChange={setOpenDeleteDialog}
        deleteFn={() => {
          reinitializeFn();
          setOpenDeleteDialog(false);
        }}
      />
    </>
  );
}
