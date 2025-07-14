interface TabProps {
  children: any;
  activeTab: number;
  currentTab: number;
  setActiveTab: React.Dispatch<React.SetStateAction<number>>;
  showBadge: boolean;
  resetBadge: () => void;
}

export function Tab({
  children,
  activeTab,
  currentTab,
  setActiveTab,
  showBadge,
  resetBadge,
}: TabProps) {
  return (
    <div
      onClick={() => {
        setActiveTab(currentTab);
        resetBadge();
      }}
      className={`relative px-6 py-3 rounded-lg font-medium transition-colors duration-200 cursor-pointer 
        ${
          activeTab === currentTab
            ? "bg-zinc-800 text-white shadow-inner"
            : "bg-zinc-600 text-zinc-200 hover:bg-zinc-700 hover:text-white"
        }`}
    >
      {showBadge && (
        <div className="absolute -top-1 -right-1 size-6 bg-red-600 text-white text-xs flex items-center justify-center rounded-full">
          !
        </div>
      )}
      {children}
    </div>
  );
}
