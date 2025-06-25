interface TabProps {
  children: any;
  activeTab: number;
  currentTab: number;
  setActiveTab: React.Dispatch<React.SetStateAction<number>>;
}

export function Tab({
  children,
  activeTab,
  currentTab,
  setActiveTab,
}: TabProps) {
  return (
    <div
      onClick={() => setActiveTab(currentTab)}
      className={`px-6 py-3 rounded-lg font-medium transition-colors duration-200 cursor-pointer 
        ${
          activeTab === currentTab
            ? "bg-zinc-800 text-white shadow-inner"
            : "bg-zinc-600 text-zinc-200 hover:bg-zinc-700 hover:text-white"
        }`}
    >
      {children}
    </div>
  );
}
